namespace Blog.Application.Features.User.DTO
{
    public class UserListDTO
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
