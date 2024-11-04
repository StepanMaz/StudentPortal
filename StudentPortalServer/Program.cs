using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using StackExchange.Redis;
using StudentPortalServer.Config;
using StudentPortalServer.Middleware;
using StudentPortalServer.Services;
using StudentPortalServer.UI;
using FluentValidation.AspNetCore;
using FluentValidation;
using StudentPortalServer.Models;
using StudentPortalServer.Serialization;

var builder = WebApplication.CreateBuilder(args);

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
IDatabase db = redis.GetDatabase();

builder.Services.AddSingleton(db);
builder.Services.AddSingleton<IAsyncKeyValueStorage>(x => new RedisKeyValueStorage(x.GetService<IDatabase>()!));
builder.Services.AddDbContext<StudentPortalDBContext>(x =>
{
    SPComponentSerializer.Setup();
    var config = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>()!;
    var client = new MongoClient(config.ConnectionString);
    x.UseMongoDB(client, config.DatabaseName);
});
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PageService>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>().AddFluentValidationAutoValidation();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.UseAntiforgery().UseStaticFiles();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
