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
        //services.AddScoped(typeof(IBlogRepository<>), typeof(EFBlogRepository<>));

        //cached repository
        services.AddScoped(typeof(IBlogRepository<>), typeof(CachedBlogRepository<>));
        services.AddScoped(typeof(EFBlogRepository<>));

        services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<IdentityEFContext>();

        services.Configure<IdentityOptions>(opts =>
        {
            opts.Password.RequireDigit = false;
            opts.Password.RequireUppercase = false;
            opts.Password.RequireLowercase = false;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequiredLength = 5;
            opts.User.RequireUniqueEmail = true;
        });

        services.AddScoped<IUserService, UserService>();
        return services;
    }
}
