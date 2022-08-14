using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using MiniBlog.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace MiniBlog.Tests.Config;

public class CustomWebAppFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptors = services.Select(s => s).Where(s => s.ServiceType == typeof(DbContextOptions<MiniBlogEfContext>)
                            || s.ServiceType == typeof(DbContextOptions<IdentityEfContext>)).ToList();
                            
            foreach (var descr in descriptors)
            {
                services.Remove(descr);
            }

            services.AddDbContext<MiniBlogEfContext>(opts =>
            {
                opts.UseInMemoryDatabase("MiniBlogEfInMemory");
            });

            services.AddDbContext<IdentityEfContext>(opts =>
            {
                opts.UseInMemoryDatabase("IdentityEfInMemory");
            });

            services.AddAntiforgery(opts => {
                opts.Cookie.Name = AntiForgeryExtractor.AntiforgeryCookieName;
                opts.FormFieldName = AntiForgeryExtractor.AntiforgeryFormFieldName;
            });

        });

        builder.UseEnvironment("Testing");
    }
}