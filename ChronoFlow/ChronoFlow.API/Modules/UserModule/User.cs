namespace ChronoFlow.API.Modules.UserModule
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        
        public byte[] PasswordHash { get; set; } = new byte[32];


    }
}
