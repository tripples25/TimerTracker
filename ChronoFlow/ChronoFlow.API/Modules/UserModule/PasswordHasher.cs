using System.Security.Cryptography;
using ChronoFlow.API.Infrastructure;

namespace ChronoFlow.API.Modules.UserModule;

public class PasswordHasher
{
    //private readonly Config config;

    //private byte[] passwordSalt => Config.PasswordSalt;

    //public PasswordHasher(Config config)
    //{
    //    this.config = config;
    //}

    public string CreatePasswordHash(string password, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        hmac.Key = passwordSalt;
        byte[] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(passwordHash);
    }

    public bool VerifyPasswordHash(string password, string passwordHash, byte[] passwordSalt)
    {
        var passwordHashInBytes = Convert.FromBase64String(passwordHash);
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHashInBytes);
    }
}