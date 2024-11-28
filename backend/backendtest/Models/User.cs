using System.ComponentModel.DataAnnotations;

namespace backendtest.Models;

//
public class User
{
    public const int MAX_USERNAME_LENGTH = 50;
    public User() { }

    public User(Guid id, string userName, string email, string password)
    {
        Id = id;
        UserName = userName;
        Email = email;
        PasswordHash = password;
    }
     [Key]
    public Guid Id { get; set;  }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public static (User? User, string error) Create(Guid id, string userName, string email, string password)
    {
        var error = string.Empty;

        if (string.IsNullOrEmpty(userName) || userName.Length > MAX_USERNAME_LENGTH)
        {
            return (null, "User name is too long");
        }
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            return (null, "Invalid email address.");
        }

        if (string.IsNullOrEmpty(password) || password.Length < 8)
        {
            return (null, "Password must be at least 8 characters long.");
        }
        var user = new User(id, userName, email, password);
        return (user, error);
    }
}