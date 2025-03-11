using StudentPortal.PageEditor.Models;

namespace StudentPortal.PageEditor.Services;

public interface IReadOnlyPageStorage 
{
    public Task<PageData?> GetPage(Guid id);
}

public interface IPageStorage : IReadOnlyPageStorage
{
    public Task Save(PageData page);
}