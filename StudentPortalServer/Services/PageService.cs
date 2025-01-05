using System.Collections;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StudentPortalServer.Config;
using StudentPortalServer.Entities;
using StudentPortalServer.Entities.Page;

namespace StudentPortalServer.Services;

public class PageService(MongoClient db, IOptions<MongoDBSettings> options)
{
    private IMongoCollection<PortalPage> _pages => db.GetDatabase(options.Value.DatabaseName).GetCollection<PortalPage>("pages");
    public async Task<PortalPage> AddPageAsync(PortalPage portalPage)
    {
        await _pages.InsertOneAsync(portalPage);
        return portalPage;
    }

    public Task<PortalPage> GetPortalPageByIdAsync(ObjectId id)
    {
        return _pages.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task<PortalPage> GetPortalPageByPathAsync(Slug slug)
    {
        return _pages.Find(x => x.Slug == slug).FirstOrDefaultAsync();
    }

    public async Task UpdatePortalPage(PortalPage page)
    {
        await _pages.UpdateOneAsync(
            Builders<PortalPage>.Filter.Eq(x => x.Id, page.Id),
            Builders<PortalPage>.Update.Set(x => x.Content, page.Content)
        );
    }
}
