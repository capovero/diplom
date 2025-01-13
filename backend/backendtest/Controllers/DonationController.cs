using System.Security.Claims;
using backendtest.Interfaces;
using backendtest.Models;
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
        return Ok(donation);
    }
}