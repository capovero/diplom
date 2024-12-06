using System.Net.Mime;
using backendtest.Data;
using backendtest.Dtos.UserDto;
using backendtest.HashPassword;
using backendtest.Interfaces;
using backendtest.Mappers;
using backendtest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backendtest.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAllAsync()
        {
            return _context.Users.ToListAsync();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterAsync(CreateUserDto createUserDto)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = createUserDto.UserName,
                Email = createUserDto.Email,
                PasswordHash = PasswordHelper.HashPassword(createUserDto.Password)
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
        {
         var user = await _context.Users.FindAsync(id);
         if (user == null)
         {
             return false;
         }

         user.ToUpdateUserDto(updateUserDto);         
         _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> LoginAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == UserName);
        }
    }
}