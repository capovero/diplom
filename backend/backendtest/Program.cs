using backendtest.Data;
using backendtest.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<ApplicationContext>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



//
using var scope = app.Services.CreateScope();
var userService = scope.ServiceProvider.GetRequiredService<UserService>();

userService.AddUser();
// userService.DeleteUser();
// userService.UpdateUser();

var users = userService.GetUsers();
foreach (var user in users)
{
    Console.WriteLine($"{user.Id}. {user.Name} - {user.Age}");
}
//





app.Run();