namespace Blog.Application.Models
{
    public class User
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; } 
        public List<string> Roles { get; set; } = new List<string>();
    }
}
