using System.Net.Mime;
using backendtest.Data;
using backendtest.Dtos.ProjectDto;
using backendtest.Dtos.UserDto;
using backendtest.HashPassword;
using backendtest.Interfaces;
using backendtest.Mappers;
using backendtest.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;
        
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(ApplicationContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _context.Users.ToListAsync();
        }
        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _context.Users
                .Include(u => u.Projects)
                .ThenInclude(p => p.MediaFiles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var userProjects = user.Projects 
                .Where(p => p.Status == Status.Active)
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    GoalAmount = p.GoalAmount,
                    CreatedAt = p.CreatedAt,
                    MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
                })
                .ToList();

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Projects = userProjects
            };
        }


        
        public async Task<bool> DeleteAsync(string userId)
        {
            var guidId = Guid.Parse(userId);
            var user = await _context.Users.FindAsync(guidId);
            if(user == null)
                return false;
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RegisterAsync(CreateUserDto createUserDto)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == createUserDto.Email || u.UserName == createUserDto.UserName);
            if (exists)
            {
                return false; // Email или имя пользователя уже заняты
            }
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                PasswordHash = _passwordHasher.Generate(createUserDto.Password)
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User> UpdateAsync(string userId, UpdateUserDto updateUserDto)
        {
            var idGuid = Guid.Parse(userId);
            var user = await _context.Users.FindAsync(idGuid);
         
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.ToUpdateUserDto(updateUserDto);         
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetByName(string userName)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName)
                ?? throw new Exception("User not found");
            return (userEntity);
        }
        //АДМИНСКИЕ МЕТОДЫ 
        public async Task<bool> DeleteAdmin(Guid userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
                return false;
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<UserProfileDto> GetUserProfileForAdminAsync(Guid userId)
        {
            var user = await _context.Users
                .IgnoreQueryFilters() // Учитывает глобальные фильтры
                .Include(u => u.Projects)
                .ThenInclude(p => p.MediaFiles)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return null;

            var userProjects = user.Projects
                .Select(p => new ProjectResponseDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    GoalAmount = p.GoalAmount,
                    CreatedAt = p.CreatedAt,
                    MediaFiles = p.MediaFiles.Select(m => m.FilePath).ToList()
                })
                .ToList();

            return new UserProfileDto
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Projects = userProjects
            };

        }

    }
}