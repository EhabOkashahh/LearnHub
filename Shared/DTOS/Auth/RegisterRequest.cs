using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Auth
{
    public class RegisterRequest
    {
        [Required, StringLength(100, MinimumLength = 2)]
        public string DisplayName { get; set; } = null!;

        [Required, StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Phone]
        public string? PhoneNumber { get; set; }

        [Required, StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = null!;
    }
}
