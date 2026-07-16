namespace Shared.DTOS.Auth
{
    public class UserAuthResponse
    {
        public string Email { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Token { get; set; } = null!;
        public bool HasStudentProfile { get; set; } = true;
        public bool HasInstructorProfile { get; set; }
    }
}
