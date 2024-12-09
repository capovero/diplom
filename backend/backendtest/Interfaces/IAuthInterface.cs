namespace backendtest.Interfaces;

public interface IAuthInterface
{
    string GenerateToken(Guid id, string role);
}