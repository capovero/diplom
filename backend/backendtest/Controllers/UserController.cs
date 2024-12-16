using backendtest.Data;
using backendtest.Dtos.UserDto;
using backendtest.HashPassword;
using backendtest.Interfaces;
using backendtest.Mappers;
using backendtest.Models;
using backendtest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Controllers;
[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IUserRepository _userRepo;
    private readonly IUserService _userService;
    private readonly HttpContext _httpContext;
    public UserController(ApplicationContext context, IUserRepository userRepo, IUserService userService)
    { 
        _userRepo = userRepo;
        _context = context;   
        _userService = userService;
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
    var answer = await _userRepo.RegisterAsync(createUserDto);
    if (!answer)
    {
        return BadRequest("Не удалось зарегистрировать пользователя.");
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
    public async Task<IActionResult> LoginAsync([FromBody] LoginUserDto loginUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Некорректные данные.");
        }
    
        
       
        var token = await _userService.LoginWithGetToken(loginUserDto.UserName, loginUserDto.Password);
            HttpContext.Response.Cookies.Append("token", token);
            return Ok(new { Token = token });
    }
    [HttpGet("test-endpoint")]
    [Authorize]
    public IActionResult TestEndpoint()
    {
        return Ok(new { Message = "You are authorized!" });
    }
    [HttpGet("debug-token")]
    public IActionResult DebugToken()
    {
        var token = Request.Cookies["token"];
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Token not found in cookies.");
        }

        return Ok(new { token });
    }

}