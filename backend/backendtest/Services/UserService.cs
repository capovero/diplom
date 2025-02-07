using backendtest.Dtos.UserDto;
using backendtest.Interfaces;
using backendtest.Models;
using backendtest.Services;
using Microsoft.AspNetCore.Identity;

namespace backendtest.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Result<string>> LoginAsync(LoginUserDto dto)
    {
        var user = await _userRepository.GetByName(dto.UserName);
        if (user == null)
            return Result<string>.Failure(new Error("User not found"));

        var isValid = _passwordHasher.Verify(dto.Password, user.PasswordHash);
        if (!isValid)
            return Result<string>.Failure(new Error("Invalid password"));

        var token = _jwtProvider.GenerateToken(user);
        return Result<string>.Success(token);
    }
}