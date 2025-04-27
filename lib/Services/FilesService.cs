using System.Net.Http.Json;

namespace StudentPortal.Services;

#nullable disable
public class SPFileInfo
{
    public string Name { get; set; }
    public string Id { get; set; }
}
#nullable restore

public interface IFileService
{
    public Task<IEnumerable<SPFileInfo>> GetFiles();
}

internal class FileService(HttpClient httpClient) : IFileService
{
    public async Task<IEnumerable<SPFileInfo>> GetFiles()
    {
        var res = await httpClient.GetAsync("list");

        return (await res.Content.ReadFromJsonAsync<List<SPFileInfo>>())!;
    }
}