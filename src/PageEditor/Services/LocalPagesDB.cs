using CloudNimble.BlazorEssentials.IndexedDb;
using Microsoft.JSInterop;
using StudentPortal.PageEditor.Models;
using StudentPortal.PageEditor.Services;

namespace StudentPortal.PageEditor.DB;

public class LocalPagesDB : IndexedDbDatabase, IPageStorage
{
    [Index(Name = "Id", Path = "Id")]
    public IndexedDbObjectStore Pages { get; } = null!;

    public LocalPagesDB(IJSRuntime jsRuntime) : base(jsRuntime)
    {
        Name = "Pages";
        Version = 1;
    }

    public async Task Save(PageData page)
    {
        await Pages.AddAsync(page);
    }

    public Task<PageData?> GetPage(Guid id)
    {
        return Pages.GetAsync<Guid, PageData>(id);
    }
}