using StudentPortal.QuizService.DB;
using StudentPortal.QuizService.Services;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Auth;
using Dumpify;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<QuizContext>((services, options) =>
{
    options.UseNpgsql(builder.Configuration["QUIZ_SERVICE_POSTGRES_CS"]);
});
builder.Services.AddControllers();
builder.Services.AddScoped<QuizDataService>();
builder.ConfigureAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();

app.Run();