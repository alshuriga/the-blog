namespace Blog.Application.Features.Account.DTO;

public class UserSignUpDTO
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string RepeatPassword { get; set; } = null!;
}
