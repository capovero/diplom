using backendtest.Data;
using backendtest.Dtos.UserDto;
using backendtest.Interfaces;
using backendtest.Models;
using backendtest.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;
    private readonly IPasswordHasher _hasher;

    public UserRepository(ApplicationContext context, IPasswordHasher hasher)
    {
        _context = context;
        _hasher = hasher;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<Result<User>> RegisterAsync(CreateUserDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return Result<User>.Failure(new Error("Email уже существует"));

        var user = new User(
            Guid.NewGuid(),
            dto.UserName,
            dto.Email,
            _hasher.Generate(dto.Password),
            "User"
        );

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return Result<User>.Success(user);
    }

    public async Task<Result<User>> UpdateAsync(Guid id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) 
            return Result<User>.Failure(new Error("Пользователь не найден"));

        user.UserName = dto.UserName ?? user.UserName;
        user.Email = dto.Email ?? user.Email;

        await _context.SaveChangesAsync();
        return Result<User>.Success(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User?> GetProfileAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Projects)
            .ThenInclude(p => p.MediaFiles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetAdminProfileAsync(Guid id)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .Include(u => u.Projects)
            .ThenInclude(p => p.MediaFiles)
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    public async Task<User?> GetByName(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }
    
   public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) 
            return false;
        
        var userDonations = await _context.Donations
            .Where(d => d.UserId == id)
            .ToListAsync();

        foreach (var don in userDonations)
        {
            var proj = await _context.Projects.FindAsync(don.ProjectId);
            if (proj != null)
            {
                proj.CollectedAmount -= don.Amount;
                if (proj.CollectedAmount < 0) 
                    proj.CollectedAmount = 0;
                _context.Projects.Update(proj);
            }
        }
        _context.Donations.RemoveRange(userDonations);
        
        var userReviews = await _context.Reviews
            .Where(r => r.UserId == id)
            .ToListAsync();
        
        var affectedProjectIds = userReviews
            .Select(r => r.ProjectId)
            .Distinct()
            .ToList();
        
        _context.Reviews.RemoveRange(userReviews);
        
        foreach (var pid in affectedProjectIds)
        {
            var remainingReviews = await _context.Reviews
                .Where(r => r.ProjectId == pid)
                .ToListAsync();

            var proj = await _context.Projects.FindAsync(pid);
            if (proj != null)
            {
                if (remainingReviews.Any())
                {
                    proj.AverageRating = (float)remainingReviews.Average(r => r.Rating);
                }
                else
                {
                    proj.AverageRating = 0;
                }
                _context.Projects.Update(proj);
            }
        }
        await _context.SaveChangesAsync();
        
        var userProjects = await _context.Projects
            .Where(p => p.UserId == id)
            .ToListAsync();

        foreach (var p in userProjects)
        {
            var donationsOfProject = await _context.Donations
                .Where(d => d.ProjectId == p.Id)
                .ToListAsync();
            _context.Donations.RemoveRange(donationsOfProject);
            
            var reviewsOfProject = await _context.Reviews
                .Where(r => r.ProjectId == p.Id)
                .ToListAsync();
            _context.Reviews.RemoveRange(reviewsOfProject);
            
            _context.Projects.Remove(p);
        }
        
        _context.Users.Remove(user);
        
        await _context.SaveChangesAsync();
        return true;
    }
}