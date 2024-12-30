using System.Security.Cryptography;

namespace UserAuthEntities.InternalUsers;

internal class PasswordHelper
{
    internal static byte[] GenerateSalt(int size = 32)
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }

    // Method to hash a password with a salt using PBKDF2
    internal static string HashPassword(string password, byte[] salt)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256)) // 10,000 iterations
        {
            byte[] hash = pbkdf2.GetBytes(32); // 32 bytes for the hash
            return Convert.ToBase64String(hash);
        }
    }

    // Method to verify if the entered password matches the stored hash
    internal static bool VerifyPassword(string enteredPassword, string storedHash, byte[] storedSalt)
    {
        string enteredHash = HashPassword(enteredPassword, storedSalt);
        return enteredHash == storedHash;
    }
}