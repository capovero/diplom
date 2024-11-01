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
        var user1 = new User{Name = "user", Age = 16};
        var user2 = new User {Name = "lix", Age = 38};
        
        _context.Users.AddRange(user1, user2);
        _context.SaveChanges();
    }

    public void DeleteUser()
    {
        User? user = _context.Users.FirstOrDefault();
        _context.Users.Remove(user);
        _context.SaveChanges();
    }

    public void UpdateUser()
    {
        User? user = _context.Users.FirstOrDefault();
        Console.WriteLine("da");
        // string newname = 
        user.Name = "Dmitri";
        user.Age = 40;
        _context.SaveChanges();
    }
    public List<User> GetUsers()
    {
        return _context.Users.ToList();
    }
}
