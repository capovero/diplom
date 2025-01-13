using backendtest.Dtos;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IDonationRepository 
{
    Task<DonationResponceDto> CreateDonation(int projectId, string userId, decimal amount);
    Task CheckAndUpdateStatus(int projectId);
}