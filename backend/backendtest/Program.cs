using System.Text;
using backendtest;
using backendtest.Configurations;
using backendtest.Data;
using backendtest.HashPassword;
using backendtest.Interfaces;
using backendtest.Repository;
using backendtest.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
var jwtOptions = builder.Configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
                 ?? throw new InvalidOperationException("JwtOptions section is missing or invalid in configuration.");
builder.Services.AddApiAuthentication(jwtOptions);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Set-Cookie"); 
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddScoped<IUpdateRepository, UpdateRepository>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});


app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = app.Environment.IsDevelopment() 
        ? CookieSecurePolicy.None 
        : CookieSecurePolicy.Always
});

app.UseCors("DevCors");  


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseRouting();
app.UseStaticFiles();
app.UseCors("DevCors"); 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
