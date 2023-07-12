using Blog.Application.Constants;
using Blog.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MiniBlog.Infrastructure.Data;

namespace Blog.Infrastructure.DataSeed;

public static class SeedData
{
    public static void EnsureSeedContent(IServiceProvider services)
    {
        var appDb = services.CreateScope().ServiceProvider.GetRequiredService<BlogEFContext>();
        appDb.Database.EnsureCreated();

        //if (appdb.database.isrelational() && appdb.database.getpendingmigrations().any())
        //{
        //    appdb.database.migrate();
        //}

        //if (!appdb.posts.any() && !appdb.commentaries.any() && !appdb.tags.any())
        //{
        //    console.writeline("started seeding database with sql query...");
        //    using var file = file.open("../../seedqueries/miniblog.sql", filemode.open);
        //    using var reader = new streamreader(file);
        //    using var transaction = appdb.database.begintransaction();
        //    appdb.database.executesqlraw(reader.readtoend());
        //    transaction.commit();
        //    console.writeline("finished seeding database.");
        //}
    }




    public static async Task EnsureSeedIdentity(IServiceProvider services)
    {

        var appDb = services.CreateScope().ServiceProvider.GetRequiredService<IdentityEFContext>();


        appDb.Database.EnsureCreated();

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

            await userManager.AddToRoleAsync(await userManager.FindByNameAsync("admin"), adminsRole.Name);


            IdentityUser normalUser = new IdentityUser
            {
                UserName = "normal",
                Email = "normal@basic.com"
            };
            await userManager.CreateAsync(normalUser, "12345");
        }

    }
}




