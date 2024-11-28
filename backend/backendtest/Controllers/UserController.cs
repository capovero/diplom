using backendtest.Data;
using backendtest.Dtos.UserDto;
using backendtest.HashPassword;
using backendtest.Mappers;
using backendtest.Models;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationContext _context;
    public UserController(ApplicationContext context)
    {
     _context = context;   
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _context.Users.ToList()
            .Select(u => u.ToUserDto());
        return Ok(users);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        var user = _context.Users.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToUserDto());
    }

    [HttpPost("register")]
    public IActionResult Register(CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Некорректные данные.");
        }
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            UserName = createUserDto.UserName,
            Email = createUserDto.Email,
            PasswordHash = PasswordHelper.HashPassword(createUserDto.Password)
        };
        _context.Users.Add(newUser);
        _context.SaveChanges();

        return Ok("Пользователь успешно зарегистрирован!");
    }
    
}