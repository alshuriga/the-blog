using Blog.Infrastructure.Data;
using Blog.IntegrationTests.WebAPI.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;

namespace Blog.IntegrationTests.WebAPI
{
    public class TestWebAppFactory<TEntry> : WebApplicationFactory<TEntry> where TEntry : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Production");
            builder.ConfigureServices(services =>
            {
                var descriptors = services.Where(x =>
                x.ServiceType == typeof(DbContextOptions<BlogEFContext>)
                || x.ServiceType == typeof(DbContextOptions<IdentityEFContext>)
                || x.ServiceType == typeof(IDistributedCache)).ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDistributedMemoryCache();

                services.AddDbContext<BlogEFContext>(opts => opts.UseInMemoryDatabase("BlogDB"));
                services.AddDbContext<IdentityEFContext>(opts => opts.UseInMemoryDatabase("IdentityDB"));


                var provider = services.BuildServiceProvider();

                var dbService = provider.GetRequiredService<BlogEFContext>();

                dbService.EnsureCreatedAndSeeded();

            });

            base.ConfigureWebHost(builder);
        }

   

    }
}
