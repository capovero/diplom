using backendtest.Interfaces;
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
    
}