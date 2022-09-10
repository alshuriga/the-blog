using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.IntegrationTests.MVC.Mocks
{
    public class MockAuthClaimsOptions
    {
        public Claim[] Claims { get; set; } = Array.Empty<Claim>();
    }
}
