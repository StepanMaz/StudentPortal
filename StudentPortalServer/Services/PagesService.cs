using StudentPortalServer.Entities;

namespace StudentPortalServer.Services;

public class PageService(StudentPortalDBContext db) {
    public async Task<PortalPage> AddPageAsync(PortalPage portalPage) {
        db.Pages.Add(portalPage);
        await db.SaveChangesAsync();
        return portalPage;
    }
}
