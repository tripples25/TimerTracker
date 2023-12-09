using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.Modules.UserModule
{
    public class UserLogInRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Длина пароля минимум 6 символов")]
        public string Password { get; set; } = string.Empty;
    }

}
