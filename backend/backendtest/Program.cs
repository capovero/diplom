using backendtest.Data;
using backendtest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<ApplicationContext>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



using var scope = app.Services.CreateScope();
var userService = scope.ServiceProvider.GetRequiredService<UserService>();



app.Run();