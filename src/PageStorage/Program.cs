using FluentValidation;

using Microsoft.Extensions.Options;

using MongoDB.Bson.Serialization;
using MongoDB.Driver;

using StudentPortal.Auth;
using StudentPortal.ComponentData;
using StudentPortal.ComponentData.Serialization;
using StudentPortal.PageStorage.Config;
using StudentPortal.PageStorage.Serialization;
using StudentPortal.PageStorage.Services;

BsonSerializer.RegisterSerializer(typeof(ComponentVersion), new ComponentVersionBsonConverter());

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new ComponentVersionJsonConverter());
    });
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton((sp) =>
{
    var config = sp.GetRequiredService<IOptions<MongoConfig>>().Value;
    return new MongoClient(config.ConnectionString);
});
builder.Services.AddSingleton((sp) =>
{
    var config = sp.GetRequiredService<IOptions<MongoConfig>>().Value;
    var client = sp.GetRequiredService<MongoClient>();
    var database = client.GetDatabase(config.DatabaseName);

    return database;
});
builder.Services.AddSingleton<KeyService>();
builder.Services.AddSingleton<PageService>();
builder.Services.AddCors();
builder.ConfigureAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.UseHttpsRedirection();

app.UseCors(x => x
         .AllowAnyMethod()
         .AllowAnyHeader()
         .SetIsOriginAllowed(origin => true)
         .AllowCredentials());

app.Run();