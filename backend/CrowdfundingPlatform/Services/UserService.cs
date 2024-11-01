using CrowdfundingPlatformBackend.Models;
using CrowdfundingPlatformBackend.Data;
namespace CrowdfundingPlatformBackend.Services;

public class UserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }

    public void AddUser()
    {
        var user1 = new User{Name = "Test", Age = 20};
        var user2 = new User {Name = "nn", Age = 40};
        
        _context.Users.AddRange(user1, user2);
        _context.SaveChanges();
    }

    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }
}
