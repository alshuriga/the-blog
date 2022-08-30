using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using MiniBlog.Core.Constants;

namespace MiniBlog.Infrastructure.DataSeed;

public static class SeedData
{
    public static void EnsureSeedContent(IServiceProvider services)
    {
        var appDb = services.CreateScope().ServiceProvider.GetRequiredService<MiniBlogEfContext>();

        if (appDb.Database.IsRelational() && appDb.Database.GetPendingMigrations().Any())
        {
            appDb.Database.Migrate();
        }

        if (!appDb.Posts.Any() && !appDb.Commentaries.Any() && !appDb.Tags.Any())
        {
            Console.WriteLine("Started seeding database with SQL query...");
            using var file = File.Open("../../seedqueries/MiniBlog.sql", FileMode.Open);
            using var reader = new StreamReader(file);
            using var transaction = appDb.Database.BeginTransaction();
            appDb.Database.ExecuteSqlRaw(reader.ReadToEnd());
            transaction.Commit();
            Console.WriteLine("Finished seeding database.");
        }

    }

    public static async Task EnsureSeedIdentity(IServiceProvider services)
    {
        services = services.CreateScope().ServiceProvider;
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        if (!roleManager.Roles.Any() && !userManager.Users.Any())
        {
            IdentityRole adminsRole = new IdentityRole(RolesConstants.ADMIN_ROLE);
            await roleManager.CreateAsync(adminsRole);

            IdentityUser adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };

            await userManager.CreateAsync(adminUser, "admin");
            await userManager.AddToRoleAsync(adminUser, adminsRole.Name);


            IdentityUser normalUser = new IdentityUser
            {
                UserName = "normal",
                Email = "normal@basic.com"
            };
            await userManager.CreateAsync(normalUser, "12345");
        }
    }


}
