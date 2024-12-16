using backendtest.Models;

namespace backendtest.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
}