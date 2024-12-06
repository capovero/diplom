namespace backendtest.Interfaces;

public interface IAuthInterface
{
    string GenerateToken(string username, string role);
}