using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudExtensions.Services;
using PageEditor;
using StudentPortal.ComponentData.Conversion;
using StudentPortal.Services;
using Fluxor;
using StudentPortal.PageEditor;
using StudentPortal.ComponentData.Quizzes;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
builder.Services.AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddSingleton(new DocumentConverter());
builder.Services.AddSingleton(new ComponentDataConverter(TypeRegistry.AssemblyBased));
builder.Services.AddScoped<IQuizManager, EditorQuizManager>();
builder.Services.AddFluxor(x => x.ScanAssemblies(typeof(Program).Assembly));

builder.Services.AddPageStorageService($"{new Uri(builder.HostEnvironment.BaseAddress).GetLeftPart(UriPartial.Authority)}/api/pages/");

var app = builder.Build();

app.Services.GetRequiredService<NavigationManager>().ToAbsoluteUri("/pages/editor/");

await app.RunAsync();
