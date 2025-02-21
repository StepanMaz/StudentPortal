using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StudentPortal.ComponentData;

namespace StudentPortal.Services.PageStorage;

public interface IPageStorageService
{
    Task<bool> CreatePage(Document document);
    Task<PageData?> GetPage(Guid id);
    Task<bool> UpdatePage(Guid id, Dictionary<string, object>? metadata = null, Document? content = null);
    Task<bool> DeletePage(Guid id);
}

public class PageStorageService(Uri baseAddress, IOptions<JsonOptions> jsonOptions, ILogger<PageStorageService> logger) : IPageStorageService
{
    private readonly JsonSerializerOptions _serializerOptions = jsonOptions.Value.JsonSerializerOptions;
    private HttpClient _client = new HttpClient() { BaseAddress = baseAddress };

    public PageStorageService(string baseAddress, IOptions<JsonOptions> jsonOptions, ILogger<PageStorageService> logger) : this(new Uri(baseAddress), jsonOptions, logger) { }


    public async Task<bool> CreatePage(Document document)
    {
        var res = await _client.PostAsJsonAsync("/", new { Content = document });

        return res.IsSuccessStatusCode;
    }

    public async Task<PageData?> GetPage(Guid id)
    {
        var res = await _client.GetAsync(id.ToString());

        if (!res.IsSuccessStatusCode)
        {
            return null;
        }

        try
        {
            var pageData = await res.Content.ReadFromJsonAsync<PageData>(_serializerOptions);

            return pageData;
        }
        catch (Exception e)
        {
            logger.LogWarning(e, "Could not parse json");

            return null;
        }
    }

    public async Task<bool> UpdatePage(Guid id, Dictionary<string, object>? metadata = null, Document? content = null)
    {
        var res = await _client.PutAsJsonAsync(id.ToString(), new { Metadata = metadata, Content = content }, _serializerOptions);

        return res.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePage(Guid id)
    {
        var res = await _client.DeleteAsync(id.ToString());

        return res.IsSuccessStatusCode;
    }
}


#nullable disable
public class PageData
{
    public Guid Id { get; set; }
    public Document Content { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
}
#nullable enable

public static class PageStorageServiceDIExtension
{
    public static IServiceCollection AddPageStorageService(this IServiceCollection services, string baseAddress)
    {
        services.AddSingleton<IPageStorageService, PageStorageService>(sp =>
        {
            var jsonOptions = sp.GetRequiredService<IOptions<JsonOptions>>();

            var logger = sp.GetRequiredService<ILogger<PageStorageService>>();

            return new PageStorageService(baseAddress, jsonOptions, logger);
        });

        return services;
    }
}