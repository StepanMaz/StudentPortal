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
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using StudentPortalServer.Serialization;

BsonSerializer.RegisterSerializer(new SPComponentSerializer());
BsonSerializer.RegisterSerializer(new SlugSerializer());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(x =>
{
    ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
    IDatabase db = redis.GetDatabase();
    return db;
});
builder.Services.AddSingleton<IAsyncKeyValueStorage>(x => new RedisKeyValueStorage(x.GetService<IDatabase>()!));

builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton(services =>
{
    var config = services.GetRequiredService<IOptions<MongoDBSettings>>();
    var client = new MongoClient(config.Value.ConnectionString);
    return client;
});
builder.Services.AddDbContext<StudentPortalDBContext>((services, options) =>
{
    var config = services.GetRequiredService<IOptions<MongoDBSettings>>();
    var client = services.GetRequiredService<MongoClient>();
    options.UseMongoDB(client, config.Value.DatabaseName);
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
