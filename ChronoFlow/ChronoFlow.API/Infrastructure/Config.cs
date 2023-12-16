using System.Security.Cryptography;

namespace ChronoFlow.API.Infrastructure;

public class Config
{
    public string DatabaseConnectionString { get; init; }
    public byte[] PasswordSalt { get; } = new HMACSHA512().Key;

    public Config(bool isDev)
    {
        if (isDev)
        {
            DatabaseConnectionString = "Server=localhost;Database=TimeTrackerDB;Port=5432;User Id=postgres;Password=1";
        }
        else
        {
            DatabaseConnectionString = Environment.GetEnvironmentVariable("Connection");
        }
    }
}