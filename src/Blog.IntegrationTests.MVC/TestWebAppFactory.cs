
using Blog.Infrastructure.Data;
using Blog.IntegrationTests.MVC.Mocks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;
using System.Security.Claims;


namespace Blog.IntegrationTests.MVC
{
    public class TestWebAppFactory<TEntry> : WebApplicationFactory<TEntry> where TEntry : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Production");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<BlogEFContext>));
                if (descriptor != null) services.Remove(descriptor);

                descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<IdentityEFContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<BlogEFContext>(opts => opts.UseInMemoryDatabase("BlogDB"));
                services.AddDbContext<IdentityEFContext>(opts => opts.UseInMemoryDatabase("IdentityDB"));


                var provider = services.BuildServiceProvider();

                var dbService = provider.GetRequiredService<BlogEFContext>();

                dbService.EnsureCreatedAndSeeded();

            });

            base.ConfigureWebHost(builder);
        }

        public HttpClient CreateClientWithAuth(Claim[] claims, WebApplicationFactoryClientOptions? options = null)
        {
            var factory = this.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.Configure<MockAuthClaimsOptions>(opts =>
                    {
                        opts.Claims = claims;
                    });
                    services.AddAuthentication(opts =>
                    {
                        opts.DefaultAuthenticateScheme = "Test";
                    }).AddScheme<AuthenticationSchemeOptions, MockAuthHandler>("Test", options => { });
                });
            });

            if (options != null)
                return factory.CreateClient(options);

            return factory.CreateClient();
        }

    }
}
