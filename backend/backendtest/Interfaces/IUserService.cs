namespace backendtest.Interfaces;

public interface IUserService
{
    Task<string> LoginWithGetToken(string userName, string password);
}