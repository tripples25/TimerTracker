using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.Modules.UserModule
{
    public class UserRegisterRequest
    {   
        [Required, MinLength(4), MaxLength(16)] 
        public string Name { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Длина пароля минимум 6 символов")]
        public string Password { get; set; } = string.Empty;

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}