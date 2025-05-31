using Microsoft.AspNetCore.Mvc;
using PageView.Components;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.ComponentData.Serialization;
using StudentPortal.Services;
using StudentPortal.Auth;
using MudBlazor.Services;
using StudentPortal.PageView.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddPageStorageService("http://page-storage:5000").AddQuizService("http://quiz-service:5000");

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ComponentVersionJsonConverter());
});
builder.Services.AddSingleton(new DocumentConverter());
builder.Services.AddSingleton(new ComponentDataConverter(TypeRegistry.AssemblyBased));
builder.Services.AddSingleton<IAsyncKeyValueStorage<string, string>>(o =>
{
    var connectionMultiplexer = ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!);

    var database = connectionMultiplexer.GetDatabase();

    return new RedisTemporaryStorage(database);
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddMudServices();
builder.ConfigureAuth();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
