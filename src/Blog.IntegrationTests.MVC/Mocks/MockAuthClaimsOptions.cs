using System.Security.Claims;

namespace Blog.IntegrationTests.MVC.Mocks
{
    public class MockAuthClaimsOptions
    {
        public Claim[] Claims { get; set; } = Array.Empty<Claim>();
    }
}
