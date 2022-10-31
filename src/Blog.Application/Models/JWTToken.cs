namespace Blog.WebAPI.Models;

public class JwtToken
{
    public string Token { get; set; } = null!;

    public JwtToken(string token)
    {
        Token = token;
    }
}
