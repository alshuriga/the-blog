using Blog.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Blog.Application.Interfaces.Common;
using Blog.Application.Interfaces;
using Blog.Infrastructure.Identity;
using MiniBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Blog.Infrastructure;

public static class Configuration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlogEFContext>(opts =>
        {
            opts.UseSqlServer(configuration.GetConnectionString("BlogDatabase"));
        });
        services.AddDbContext<IdentityEFContext>(opts =>
        {
            opts.UseSqlServer(configuration.GetConnectionString("IdentityDatabase"));
        });
        services.AddScoped(typeof(IBlogRepository<>), typeof(EFBlogRepository<>));
        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityEFContext>();
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
