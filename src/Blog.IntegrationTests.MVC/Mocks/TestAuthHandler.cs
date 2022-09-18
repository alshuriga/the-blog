using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Blog.IntegrationTests.MVC.Mocks
{
    public class MockAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly MockAuthClaimsOptions _claims;

        public MockAuthHandler(IOptions<MockAuthClaimsOptions> claims, IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _claims = claims.Value;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = _claims.Claims;

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");
            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
