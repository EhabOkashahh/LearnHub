namespace Shared.DTOS.Users
{
    public class UpdateUserRequest
    {
        public string? DisplayName { get; set; }
        public string? HeadLine { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
