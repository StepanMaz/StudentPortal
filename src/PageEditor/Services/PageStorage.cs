using StudentPortal.PageEditor.Models;

namespace StudentPortal.PageEditor.Services;

public class PageStorage : IPageStorage
{
    public Task<PageData?> GetPage(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task Save(PageData page)
    {
        throw new NotImplementedException();
    }
}
