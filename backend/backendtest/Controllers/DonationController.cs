using System.Security.Claims;
using backendtest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;

[Route("api/donation")]
[ApiController]
public class DonationController : ControllerBase
{
    private readonly IDonationRepository _donationRepository;

    public DonationController(IDonationRepository donationRepository)
    {
        _donationRepository = donationRepository;
    }

    [HttpPost("create-donate")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> CreateDonation(int projectId, decimal amount)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var donation = await _donationRepository.CreateDonation(projectId, userId, amount);
        if (donation == null)
            return BadRequest("Ошибка создания доната.");

        return Ok(donation);
    }

    [HttpGet("donations-for-creator-project")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> GetDonationsForProject(int projectId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("Не удалось определить текущего пользователя.");

        var donations = await _donationRepository.GetDonationsByProjectForOwnerAsync(projectId, userId);
        if (donations == null || !donations.Any())
            return NotFound("Донаты для этого проекта отсутствуют.");

        return Ok(donations);
    }

    [HttpGet("personal-donations")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> GetDonationsForUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized("Не удалось определить текущего пользователя.");

        var donations = await _donationRepository.GetDonationsByUserAsync(userId);
        if (donations == null || !donations.Any())
            return NotFound("У вас нет пожертвований.");

        return Ok(donations);
    }

    [HttpGet("admin-get-project-donations")]
    [Authorize("AdminPolicy")]
    public async Task<IActionResult> GetDonationsForAdmin(int projectId)
    {
        var donations = await _donationRepository.GetDonationsByProjectForAdminAsync(projectId);
        if (donations == null || !donations.Any())
            return NotFound("Донаты для проекта не найдены.");

        return Ok(donations);
    }

    [HttpGet("personal-donations-user-for-admin")]
    [Authorize("AdminPolicy")]
    public async Task<IActionResult> GetDonationsForUserByAdmin(string userId)
    {
        var donations = await _donationRepository.GetDonationsByUserForAdminAsync(userId);
        if (donations == null || !donations.Any())
            return NotFound("Пожертвования для указанного пользователя не найдены.");

        return Ok(donations);
    }
}
