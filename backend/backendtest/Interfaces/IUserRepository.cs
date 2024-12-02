using backendtest.Models;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<bool> DeleteAsync(Guid id);
}