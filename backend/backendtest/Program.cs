using System.Text;
using backendtest;
using backendtest.Configurations;
using backendtest.Data;
using backendtest.HashPassword;
using backendtest.Interfaces;
using backendtest.Repository;
using backendtest.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настраиваем JwtOptions
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

// Получаем доступ к JwtOptions через Options
builder.Services.AddApiAuthentication(
    builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>()
);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();



var app = builder.Build();
app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Включение Swagger в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Маршрутизация для контроллеров
app.MapControllers();

app.MapGet("get", () =>
{
    return Results.Ok("work");
}).RequireAuthorization();

app.Run();