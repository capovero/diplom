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
    private readonly IAuthInterface _auth;
    public UserController(ApplicationContext context, IUserRepository userRepo, IAuthInterface auth)
    { 
        _userRepo = userRepo;
        _context = context;   
        _auth = auth;
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userRepo.DeleteAsync(id);
        if(!result)
        {
            return NotFound();
        }
        return Ok();
        
    }
    
    [HttpPost("register")]
    public async Task <IActionResult> Register(CreateUserDto createUserDto)
    {
    if (!ModelState.IsValid)
    {
        return BadRequest("Некорректные данные.");
    }
    var resault = await _userRepo.RegisterAsync(createUserDto);
    if (!resault)
    {
        return NotFound();
    }
    return Ok("Пользователь успешно зарегистрирован!");
    }

    [HttpPut("id")]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto updateUserDto)
    {
        var result = await _userRepo.UpdateAsync(id, updateUserDto);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await _userRepo.LoginAsync(loginUserDto.UserName);
        if (user == null)
        {
            return Unauthorized();
        }

        if (!BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        var token = _auth.GenerateToken(user.Id, user.Role);
        return Ok(new {token});
    }

}