using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentPortal.AuthService;
using StudentPortal.AuthService.DB;
using StudentPortal.AuthService.Entities;
using FluentValidation.AspNetCore;
using StudentPortal.EventBusRabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.AddRabbitMQEventBus(builder.Configuration["RABBIT_MQ_CS"]!);

builder.Services.AddFluentValidationAutoValidation();

builder.Services.Configure<JWTConfig>((o) =>
{
    o.Audience = builder.Configuration["JWT_AUDIENCE"]!;
    o.Issuer = builder.Configuration["JWT_ISSUER"]!;
    o.SecretKey = builder.Configuration["JWT_SECRET"]!;
});

builder.Services.AddDbContext<AuthServiceContext>((services, options) =>
{
    options.UseNpgsql(builder.Configuration["AUTH_SERVICE_POSTGRES_CS"]);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddEntityFrameworkStores<AuthServiceContext>()
    .AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JWTConfig>()!;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanEdit", policy => policy.RequireRole("Teacher"));
    options.AddPolicy("CanView", policy => policy.RequireRole("Teacher", "Student"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
    options.LoginPath = "/auth/login";
    options.SlidingExpiration = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AuthServiceContext>();
    await context.Database.EnsureCreatedAsync();
    var seeder = new DataSeeder(services.GetRequiredService<RoleManager<IdentityRole>>());
    await seeder.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
}

app.MapGet("hello", () => "Hello!");

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
