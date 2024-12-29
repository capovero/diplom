using System.Security.Claims;
using backendtest.Data;
using backendtest.Dtos.ProjectDto;
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
    
    [Authorize("UserPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepo.GetAllAsync();
        var usersDto = users.Select(u => u.ToUserDto());
        return Ok(usersDto);
    }

    [HttpGet("{id}")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> GetUserProfile([FromRoute] Guid id)
    {
        var userProfile = await _userRepo.GetUserProfileAsync(id);
        if (userProfile == null)
            return NotFound(new { Message = "User not found." });
        return Ok(userProfile);
    }


    
    [Authorize("UserPolicy")]
    [HttpDelete("DeleteUser")]
    public async Task<IActionResult> Delete()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _userRepo.DeleteAsync(userId);
        Response.Cookies.Delete("token");
        return Ok(result);
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

    [Authorize("UserPolicy")]
    [HttpPut("id")]
    public async Task<IActionResult> Update(UpdateUserDto updateUserDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var result = await _userRepo.UpdateAsync(userId, updateUserDto);
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
    
    //АДМИНСКИЕ МЕТОДЫ
    [Authorize("AdminPolicy")]
    [HttpDelete("Admin-delete-user")]
    public async Task<IActionResult> AdminDelete(Guid userId)
    {
        var result = await _userRepo.DeleteAdmin(userId);
        if (!result)
            return BadRequest("user not found");
        return Ok(result); 
    }
    [Authorize("AdminPolicy")]
    [HttpGet("GetUserProfileForAdmin/{id}")]
    public async Task<IActionResult> GetUserProfileForAdmin([FromRoute] Guid id)
    {
        var userProfile = await _userRepo.GetUserProfileForAdminAsync(id);
        if (userProfile == null)
            return NotFound(new { Message = "User not found." });
        return Ok(userProfile);
    }

}