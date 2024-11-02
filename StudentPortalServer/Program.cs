using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using StackExchange.Redis;
using StudentPortalServer.Models.Config;
using StudentPortalServer.Services;
using StudentPortalServer.UI;

var builder = WebApplication.CreateBuilder(args);

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
IDatabase db = redis.GetDatabase();

builder.Services.AddSingleton(db);
builder.Services.AddSingleton<IAsyncKeyValueStorage>(x => new RedisKeyValueStorage(x.GetService<IDatabase>()!));
builder.Services.AddDbContext<StudentPortalDBContext>(x =>
{
    var config = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>()!;
    var client = new MongoClient(config.ConnectionString);
    x.UseMongoDB(client, config.DatabaseName);
});
builder.Services.AddScoped<UserService>();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();

app.UseAntiforgery().UseStaticFiles();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
