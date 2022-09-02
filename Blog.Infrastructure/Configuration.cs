using Blog.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces.Common;

namespace Blog.Infrastructure;

public static class Configuration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlogEFContext>(opts =>
        {
            opts.UseSqlServer(configuration.GetConnectionString("BlogDatabase"));
        });
        services.AddScoped(typeof(IBlogRepository<>), typeof(EFBlogRepository<>));

        return services;
    }
}
