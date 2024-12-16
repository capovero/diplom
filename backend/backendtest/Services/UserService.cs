using backendtest.Models;
using backendtest.Data;
using backendtest.Interfaces;
using backendtest.Repository;

namespace backendtest.Services;

public class UserService : IUserService
{
    private readonly ApplicationContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UserService(ApplicationContext context, IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    {
        _context = context;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<string> LoginWithGetToken(string userName, string password)
    {
        var user = await _userRepository.GetByName(userName);
        var resault = _passwordHasher.Verify(password, user.PasswordHash);
        if (resault == false)
        {
            throw new Exception("Invalid username or password");
        }
        var token = _jwtProvider.GenerateToken(user);
        return token;
    }

   
}