using backendtest.Data;
using backendtest.Dtos.UserDto;
using backendtest.HashPassword;
using backendtest.Interfaces;
using backendtest.Mappers;
using backendtest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IUserRepository _userRepo;
    public UserController(ApplicationContext context, IUserRepository userRepo)
    { 
        _userRepo = userRepo;
        _context = context;   
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepo.GetAllAsync();
        var usersDto = users.Select(u => u.ToUserDto());
        return Ok(usersDto);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetById([FromRoute] Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user.ToUserDto());
    }

    [HttpPost("register")]
    public async Task <IActionResult> Register(CreateUserDto createUserDto)
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
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Ok("Пользователь успешно зарегистрирован!");
    } 
    
}