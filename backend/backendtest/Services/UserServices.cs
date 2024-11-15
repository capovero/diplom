using backendtest.Models;
using backendtest.Data;

namespace backendtest.Services;

public class UserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }

   
}