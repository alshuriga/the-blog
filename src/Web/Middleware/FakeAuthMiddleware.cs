using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MiniBlog.Web.Middleware;

public class TestsFakeAuthMiddleWare
{
    private readonly RequestDelegate _next;
    public const string HeaderKey = "X-Integration-Test-Auth-Header";
    public const string AuthType = "FakeAuthType";

    public TestsFakeAuthMiddleWare(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        if(context.Request.Headers.ContainsKey(HeaderKey) && !string.IsNullOrEmpty(context.Request.Headers[HeaderKey].FirstOrDefault()))
        {
            var username = context.Request.Headers["test-username"].First();
            var email = context.Request.Headers["test-email"].First();
            var role = context.Request.Headers["test-role"].First();

            var identity = new ClaimsIdentity(
                new List<Claim> {
                    new Claim (ClaimTypes.Name, username),
                    new Claim (ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, role)
                },
                AuthType
            );
            var principal = new ClaimsPrincipal(identity);
            context.User = principal;
        } 
        await _next(context);
    }
}