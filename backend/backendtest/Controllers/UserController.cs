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

    // [Authorize]
    // [HttpGet("me")]
    // public async Task<IActionResult> GetMyProfile()
    // {
    //     var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    //     var user = await _userRepo.GetProfileAsync(userId);
    //     return Ok(user.ToAdminProfileDto());
    // }
    //
    // // [Authorize(Roles = "Admin")]
    // [AllowAnonymous]
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetAdminProfile(Guid id)
    // {
    //     var user = await _userRepo.GetAdminProfileAsync(id);
    //     return Ok(user.ToAdminProfileDto());
    // }
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProfile(Guid id)
    {
        var userEntity = await _userRepo.GetAdminProfileAsync(id);
        if (userEntity == null)
            return NotFound();
        
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isAdmin = User.IsInRole("Admin");

        if (isAdmin || currentUserId == id.ToString())
        {
            return Ok(userEntity.ToAdminProfileDto());
        }
        else
        {
            return Ok(userEntity.ToUserProfileDto());
        }
    }

    [HttpGet("me")]
    [Authorize] 
    public async Task<IActionResult> GetMyProfile()
    {
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        var guid = Guid.Parse(currentUserId);
        var userEntity = await _userRepo.GetAdminProfileAsync(guid);
        if (userEntity == null)
            return NotFound();

        return Ok(userEntity.ToAdminProfileDto());
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
            token =>
            {
                Response.Cookies.Append("token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(1)
                });
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
            Secure = Request.IsHttps,
            SameSite = SameSiteMode.Lax
        });
        return Ok(new { Message = "Successfully logged out" });
    }
    
    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserById(Guid id)
    {
        try
        {
            var success = await _userRepo.DeleteByIdAsync(id);
            if (!success)
                return NotFound("Пользователь не найден");

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Внутренняя ошибка сервера");
        }
    }
}