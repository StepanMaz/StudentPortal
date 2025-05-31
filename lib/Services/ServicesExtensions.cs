using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace StudentPortal.Services;

public static class PageStorageServiceDIExtension
{
    public static IServiceCollection AddPageStorageService(this IServiceCollection services, string baseAddress)
    {
        var client = new HttpClient() { BaseAddress = new Uri(baseAddress) };

        services.AddSingleton<IPageStorageService, PageStorageService>(sp =>
        {
            var jsonOptions = sp.GetService<JsonSerializerOptions>() ?? new JsonSerializerOptions(JsonSerializerDefaults.Web);

            var logger = sp.GetRequiredService<ILogger<PageStorageService>>();

            return new PageStorageService(client, jsonOptions, logger);
        });

        services.AddSingleton<IFileService, FileService>(sp =>
        {
            return new FileService(client);
        });

        return services;
    }

    public static IServiceCollection AddQuizService(this IServiceCollection services, string baseAddress)
    {
        var client = new HttpClient() { BaseAddress = new Uri(baseAddress) };

        services.AddSingleton<IQuizService, QuizService>(sp =>
        {
            return new QuizService(client);
        });

        return services;
    }
}