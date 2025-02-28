using CloudNimble.BlazorEssentials.IndexedDb;
using Microsoft.JSInterop;

// namespace StudentPortal.PageEditor.DB;

// public interface IPageStorage {
//     Task<Page?> GetPage(Guid guid);
// }

// public class LocalPagesDB : IndexedDbDatabase, IPageStorage
// {
//     public IndexedDbObjectStore Pages { get; } = null!;

//     public LocalPagesDB(IJSRuntime jsRuntime) : base(jsRuntime)
//     {
//         Name = "Pages";
//         Version = 1;
//     }

//     public async Task<Page?> GetPage(Guid guid)
//     {
//         return await Pages.GetAsync<Guid, Page>(guid);
//     }
// }