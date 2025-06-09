using System.Text.Json;
using MongoDB.Driver;
using StudentPortal.ComponentData;
using StudentPortal.PageStorage.Entities;

namespace StudentPortal.PageStorage.Services;

public class PageService(ILogger<PageService> logger, IMongoDatabase database, KeyService keyService)
{
    private const string CollectionName = "pages";

    private readonly IMongoCollection<Page> _pages = database.GetCollection<Page>(CollectionName);

    public async Task<Page> InsertPage(Page page)
    {
        page.Key = keyService.GetSlug(page.Name);

        await _pages.InsertOneAsync(page);

        return page;
    }

    public async Task DeletePage(Guid id, Guid userId)
    {
        var filter = Builders<Page>.Filter.Eq(x => x.Id, id) & Builders<Page>.Filter.Eq(x => x.OwnerId, userId);

        await _pages.DeleteOneAsync(filter);
    }

    public async Task UpdatePage(Page page)
    {
        var filter = Builders<Page>.Filter.Eq(p => p.Id, page.Id);

        var updates = new List<UpdateDefinition<Page>>();

        if (page.Content != null)
            updates.Add(Builders<Page>.Update.Set(p => p.Content, page.Content));

        if (page.Name != null)
        {
            updates.Add(Builders<Page>.Update.Set(p => p.Name, page.Name));
            updates.Add(Builders<Page>.Update.Set(p => p.Key, keyService.GetSlug(page.Name)));
        }

        updates.Add(Builders<Page>.Update.SetOnInsert(p => p.CreationDate, page.CreationDate));

        if (page.Metadata != null && page.Metadata.Any())
            updates.Add(Builders<Page>.Update.Set(p => p.Metadata, page.Metadata));

        if (!updates.Any())
            return;

        var update = Builders<Page>.Update.Combine(updates);
        var options = new UpdateOptions { IsUpsert = true };

        var res = await _pages.UpdateOneAsync(filter, update, options);
    }

    public async Task<Page?> GetPage(Guid id)
    {
        logger.LogTrace("Getting page {id}", id);

        var filter = Builders<Page>.Filter.Eq(x => x.Id, id);

        return await _pages.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<Page?> GetPageByKey(Guid userId, string key)
    {
        logger.LogTrace("Getting page by key {key}", key);

        var filter = Builders<Page>.Filter.Eq(x => x.Key, key) & Builders<Page>.Filter.Eq(x => x.OwnerId, userId);

        return await _pages.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Page>> GetUserPages(Guid id)
    {
        var filter = Builders<Page>.Filter.Eq(p => p.OwnerId, id);

        return await _pages.Find(filter).ToListAsync();
    }
}
