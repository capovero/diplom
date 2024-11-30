using System.Net.Mime;
using backendtest.Data;
using backendtest.Interfaces;
using backendtest.Models;
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
    }
}