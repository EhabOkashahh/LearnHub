using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = null!;
        public string? HeadLine { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public DateTime DeletedAt { get; set; }
    }
}