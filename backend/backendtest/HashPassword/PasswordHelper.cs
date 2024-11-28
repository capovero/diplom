using BCrypt.Net;

namespace backendtest.HashPassword
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Хеширует пароль перед сохранением в базу данных.
        /// </summary>
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        /// <summary>
        /// Проверяет, соответствует ли введенный пароль хешированному.
        /// </summary>
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}