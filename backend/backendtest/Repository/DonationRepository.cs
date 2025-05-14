using backendtest.Data;
using backendtest.Dtos;
using backendtest.Interfaces;
using backendtest.Models;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository;

public class DonationRepository : IDonationRepository
{
    private readonly ApplicationContext _context;

    public DonationRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<DonationResponceDto> CreateDonation(int projectId, string userId, decimal amount)
    {
        var guidUserId = Guid.Parse(userId);
        var user = await _context.Users.FindAsync(guidUserId);
        if (user == null)
            return null;

        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            return null;

        project.CollectedAmount += amount;
        project.UpdatedAt = DateTime.UtcNow;

        var donation = new Donation
        {
            Amount = amount,
            DonateAt = DateTime.UtcNow,
            ProjectId = projectId,
            UserId = guidUserId
        };

        await _context.Donations.AddAsync(donation);
        await _context.SaveChangesAsync();
        await CheckAndUpdateStatus(projectId);

        return new DonationResponceDto
        {
            Amount = donation.Amount,
            DonateAt = donation.DonateAt,
            UserName = user.UserName,
            ProjectName = project.Title
        };
    }

    public async Task CheckAndUpdateStatus(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
            return;

        if (project.CollectedAmount >= project.GoalAmount && project.Status != Status.Completed)
        {
            project.Status = Status.Completed;
            project.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<DonationResponceDto>> GetDonationsByProjectForOwnerAsync(int projectId, string ownerId)
    {
        var ownerGuid = Guid.Parse(ownerId);
        return await _context.Donations
            .Where(d => d.ProjectId == projectId && d.Project.UserId == ownerGuid)
            .Select(d => new DonationResponceDto
            {
                Amount = d.Amount,
                DonateAt = d.DonateAt,
                ProjectName = d.Project.Title,
                UserName = d.User.UserName
            })
            .ToListAsync();
    }


    public async Task<List<DonationUserDto>> GetDonationsByUserAsync(string userId)
    {
        var userGuid = Guid.Parse(userId);
        return await _context.Donations
            .Include(d => d.Project)
            .Where(d => d.UserId == userGuid)
            .Select(d => new DonationUserDto {
                Amount = d.Amount,
                DonateAt = d.DonateAt,
                ProjectTitle = d.Project.Title
            })
            .ToListAsync();

    }

    public async Task<List<DonationResponceDto>> GetDonationsByProjectForAdminAsync(int projectId)
    {
        return await _context.Donations
            .Where(d => d.ProjectId == projectId)
            .Select(d => new DonationResponceDto
            {
                Amount = d.Amount,
                DonateAt = d.DonateAt,
                ProjectName = d.Project.Title,
                UserName = d.User.UserName
            })
            .ToListAsync();
    }

    public async Task<List<DonationUserDto>> GetDonationsByUserForAdminAsync(string userId)
    {
        var userGuid = Guid.Parse(userId);
        return await _context.Donations
            .Where(d => d.UserId == userGuid)
            .Select(d => new DonationUserDto
            {
                Amount = d.Amount,
                DonateAt = d.DonateAt,
                ProjectTitle = d.Project.Title,
            })
            .ToListAsync();
    }
}
