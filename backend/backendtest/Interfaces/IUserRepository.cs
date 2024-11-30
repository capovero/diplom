using backendtest.Models;

namespace backendtest.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
}