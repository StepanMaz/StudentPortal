using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using StudentPortal.ComponentData;
using StudentPortal.ComponentData.Serialization;

namespace StudentPortal.Services;

public interface IPageStorageService
{
    Task<PageData> CreatePage(PageData pageData);
    Task<PageData?> GetPage(string id);
    Task<PageData?> GetPageByKey(string userId, string key);
    Task<bool> UpdatePage(PageData pageData);
    Task<bool> DeletePage(string id);
}

internal class PageStorageService(HttpClient httpClient, JsonSerializerOptions jsonSerializerOption, ILogger<PageStorageService> logger) : IPageStorageService
{
    public async Task<PageData> CreatePage(PageData pageData)
    {
        var res = await httpClient.PostAsJsonAsync("", new { pageData.Content, pageData.Name, pageData.Metadata }, jsonSerializerOption);

        if (!res.IsSuccessStatusCode)
        {
            logger.LogWarning("Failed to create page. Reason: {Reason}", await res.Content.ReadAsStringAsync());
            throw new Exception("Failed to upload new page");
        }

        return (await res.Content.ReadFromJsonAsync<PageData>(jsonSerializerOption))!;
    }

    public async Task<PageData?> GetPage(string id)
    {
        var res = await httpClient.GetAsync(id);

        if (!res.IsSuccessStatusCode)
        {
            logger.LogWarning("Failed to get page. Reason: {Reason}", await res.Content.ReadAsStringAsync());
            return null;
        }

        return await res.Content.ReadFromJsonAsync<PageData>(jsonSerializerOption);
    }

    public async Task<bool> UpdatePage(PageData pageData)
    {
        var res = await httpClient.PutAsJsonAsync(pageData.Id.ToString(), new { pageData.Metadata, pageData.Content, pageData.Name }, jsonSerializerOption);

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePage(string id)
    {
        var res = await httpClient.DeleteAsync(id);

        return res.IsSuccessStatusCode;
    }

    public async Task<PageData?> GetPageByKey(string userId, string key)
    {
        var res = await httpClient.GetAsync($"?key={key}&userId={userId}");

        if (!res.IsSuccessStatusCode)
        {
            logger.LogWarning("Failed to get page by key '{key}'. Reason: {Reason}", key, await res.Content.ReadAsStringAsync());
            return null;
        }

        return await res.Content.ReadFromJsonAsync<PageData>(jsonSerializerOption);
    }
}


#nullable disable
public class PageData
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string Name { get; set; }
    public string Key { get; set; }
    public Document Content { get; set; }
    [JsonConverter(typeof(PrimitiveDictionaryConverter))]
    public Dictionary<string, object> Metadata { get; set; }
    public DateTime CreationDate { get; set; }
}
#nullable enable