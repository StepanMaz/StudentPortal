using System.Text.Json;
using MongoDB.Driver;
using StudentPortal.ComponentData;
using StudentPortal.PageStorage.Entities;

namespace StudentPortal.PageStorage.Services;

public class PageService(ILogger<PageService> logger, IMongoDatabase database)
{
    private const string CollectionName = "pages";

    private readonly IMongoCollection<Page> _pages = database.GetCollection<Page>(CollectionName);

    public async Task<Page> InsertPage(Page page)
    {
        await _pages.InsertOneAsync(page);

        return page;
    }

    public async Task DeletePage(Guid id)
    {
        var filter = Builders<Page>.Filter.Eq(x => x.Id, id);

        await _pages.DeleteOneAsync(filter);
    }

    public async Task UpdatePage(Guid id, Document? content = null, IDictionary<string, object>? metadata = null)
    {
        var filter = Builders<Page>.Filter.Eq(x => x.Id, id);

        var builder = Builders<Page>.Update;
        UpdateDefinition<Page>? update = null;

        if (content is not null)
        {
            update = builder.Set(p => p.Content, content);
        }

        if (metadata is not null)
        {
            update = update == null ? builder.Set(p => p.Metadata, metadata) : update.Set(p => p.Metadata, metadata);
        }

        if (update != null)
        {
            await _pages.UpdateOneAsync(filter, update);
        }
    }

    public async Task<Page?> GetPage(Guid id)
    {
        logger.LogTrace("Getting page {id}", id);

        var filter = Builders<Page>.Filter.Eq(x => x.Id, id);

        return await _pages.Find(filter).FirstOrDefaultAsync();
    }
}
