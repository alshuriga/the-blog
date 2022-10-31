namespace Blog.Application.Models
{
    public class JwtOptions
    {
        public const string Jwt = "Jwt";

        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
    }
}
