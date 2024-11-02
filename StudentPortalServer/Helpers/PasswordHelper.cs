using System.Security.Cryptography;
using Microsoft.Extensions.ObjectPool;
using StudentPortalServer.Entities;

namespace StudentPortalServer.Helpers;

public static class PasswordHelper
{
    public static string GenerateSalt(int size = 8)
    {
        byte[] salt = new byte[size];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return Convert.ToBase64String(salt); ;
    }

    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            byte[] hash = sha256.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hash);
        }
    }
}

public static class UserPasswordExtension
{
    public static User SetPassword(this User user, string password)
    {
        var salt = PasswordHelper.GenerateSalt(8);
        var salted_password = password + salt;
        var hash = PasswordHelper.HashPassword(salted_password);

        user.Salt = salt;
        user.PasswordHash = hash;

        return user;
    }

    public static bool VerifyPassword(this User user, string password)
    {
        var salted_password = password + user.Salt;
        var hash = PasswordHelper.HashPassword(salted_password);

        return hash == user.PasswordHash;
    }
}