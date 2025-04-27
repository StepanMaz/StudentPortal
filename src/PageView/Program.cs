using Microsoft.AspNetCore.Mvc;
using PageView.Components;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.ComponentData.Serialization;
using StudentPortal.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddPageStorageService("http://localhost:5276");

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ComponentVersionJsonConverter());
});
builder.Services.AddSingleton(new DocumentConverter());
builder.Services.AddSingleton(new ComponentDataConverter(TypeRegistry.AssemblyBased));
builder.Services.AddMudServices();

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
