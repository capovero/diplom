using System.Security.Claims;
using backendtest.Dtos.UserDto;
using backendtest.Interfaces;
using backendtest.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepo;
    private readonly IUserService _userService;
    
    public UserController(IUserRepository userRepo, IUserService userService)
    {
        _userRepo = userRepo;
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userRepo.GetAllAsync();
        return Ok(users.Select(u => u.ToResponseDto()));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _userRepo.GetProfileAsync(userId);
        return Ok(user.ToAdminProfileDto());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdminProfile(Guid id)
    {
        var user = await _userRepo.GetAdminProfileAsync(id);
        return Ok(user.ToAdminProfileDto());
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _userRepo.DeleteAsync(userId);
        Response.Cookies.Delete("token");
        return result ? NoContent() : BadRequest();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
        var result = await _userRepo.RegisterAsync(dto);
    
        // Явно указываем тип IActionResult
        return result.Match<IActionResult>(
            user => CreatedAtAction(
                nameof(GetMyProfile), 
                new { id = user.Id }, 
                user.ToResponseDto()
            ),
            error => BadRequest(new { error.Message })
        );
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _userRepo.UpdateAsync(userId, dto);
    
        
        return result.Match<IActionResult>(
            user => Ok(user.ToResponseDto()),
            error => BadRequest(new { error.Message })
        );
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var result = await _userService.LoginAsync(dto);
        return result.Match<IActionResult>(
            token => {
                Response.Cookies.Append("token", token);
                return Ok(new { Token = token });
            },
            error => Unauthorized(new { error.Message })
        );
    }
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        
        Response.Cookies.Delete("token", new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.Strict
        });
    
        return Ok(new { Message = "Successfully logged out" });
    }
}