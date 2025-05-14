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
}