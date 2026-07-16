namespace ServicesAbstraction.Auth
{
    public class JwtOptions
    {
        public string TokenKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public double DurationInDays { get; set; }
    }
}
