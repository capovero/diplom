using backendtest.Dtos;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IDonationRepository
{
    Task<DonationResponceDto> CreateDonation(int projectId, string userId, decimal amount);
    Task CheckAndUpdateStatus(int projectId);
    Task<List<DonationResponceDto>> GetDonationsByProjectForOwnerAsync(int projectId, string ownerId);
    Task<List<DonationUserDto>> GetDonationsByUserAsync(string userId);
    Task<List<DonationResponceDto>> GetDonationsByProjectForAdminAsync(int projectId);
    Task<List<DonationUserDto>> GetDonationsByUserForAdminAsync(string userId);
}