using System.Security.Cryptography;

namespace ChronoFlow.API.Infra;

public class Config
{
    public string DatabaseConnectionString { get; init; }
    public static byte[] passwordSalt { get; } = new HMACSHA512().Key;

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