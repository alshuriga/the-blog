using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Common;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Data.Repositories;
using Blog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;

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

        //services.AddStackExchangeRedisCache(opts =>
        //{
        //    opts.Configuration = configuration.GetConnectionString("RedisCache");
        //    opts.InstanceName = "TheBlogCache";
        //});



        //cached repository
        //services.AddScoped(typeof(IBlogRepository<>), typeof(DistributedCacheBlogRepository<>));
        //services.AddScoped(typeof(EFBlogRepository<>));


        services.AddScoped(typeof(IBlogRepository<>), typeof(EFBlogRepository<>));

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
