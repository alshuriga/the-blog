using Blog.Application.Interfaces;
using Blog.Application.Interfaces.Common;
using Blog.Infrastructure.Data;
using Blog.Infrastructure.Data.Repositories;
using Blog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniBlog.Infrastructure.Data;

namespace Blog.Infrastructure;

public static class Configuration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<BlogEFContext>(opts =>
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
                opts.UseInMemoryDatabase("BlogDatabase");
            else
                opts.UseNpgsql(configuration.GetConnectionString("BlogDatabase")!, cfg => cfg.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromMinutes(1), errorCodesToAdd: null));

            opts.LogTo(Console.WriteLine,
                      new[] { DbLoggerCategory.Database.Command.Name },
                      LogLevel.Debug);

        });
        services.AddDbContext<IdentityEFContext>(opts =>
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
                opts.UseInMemoryDatabase("IdentityDatabase");
            else
                opts.UseNpgsql(configuration.GetConnectionString("IdentityDatabase")!, cfg => cfg.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromMinutes(1), errorCodesToAdd: null));
        }, ServiceLifetime.Transient);

        if (!configuration.GetValue<bool>("UseInMemoryDatabase"))
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


        services.AddStackExchangeRedisCache(opts =>
        {
            opts.Configuration = configuration.GetConnectionString("RedisCache");
            opts.InstanceName = "TheBlogCache";
        });



        // cached repository
        services.AddScoped(typeof(IBlogRepository<>), typeof(DistributedCacheBlogRepository<>));
        services.AddScoped(typeof(EFBlogRepository<>));


        //services.AddScoped(typeof(IBlogRepository<>), typeof(EFBlogRepository<>));

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
