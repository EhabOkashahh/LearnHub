namespace Shared.DTOS.Users
{
    public class UserResponse
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string? HeadLine { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
