using backendtest.Models;

namespace backendtest.Dtos;

public class DonationResponceDto
{
    public decimal Amount { get; set; }
    public DateTime DonateAt { get; set; } 
    public string UserName { get; set; } = string.Empty; 
    public string ProjectName { get; set; } = string.Empty; 
}