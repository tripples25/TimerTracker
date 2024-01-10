using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.Modules.UserModule.Requests
{
    public class UserChangePasswordRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Длина пароля минимум 6 символов")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required, MinLength(6, ErrorMessage = "Длина пароля минимум 6 символов")]
        public string NewPassword { get; set; } = string.Empty;

        [Required, Compare("NewPassword")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}