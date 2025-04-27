using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentPortal.Auth;
using StudentPortal.Notifications.DB;
using StudentPortal.Notifications.Hubs;
using StudentPortal.Notifications.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddTransient<INotificationsReceiver, NotificationsHub>();

builder.Services.AddScoped<NotificationsService>();

builder.Services.AddDbContext<NotificationsContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

builder.ConfigureAuth();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHub<NotificationsHub>("/Notifications");

app.UseHttpsRedirection();

app.Run();