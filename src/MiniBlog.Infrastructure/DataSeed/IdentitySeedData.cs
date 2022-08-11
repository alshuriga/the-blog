using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniBlog.Infrastructure.DataSeed;

public class IdentitySeedData
{
    public static void EnsureSeed(IServiceProvider serviceProvider)
    {
        _ensureSeed(serviceProvider).Wait();
    }
    
    private static async Task _ensureSeed(IServiceProvider serviceProvider)
    {
        serviceProvider = serviceProvider.CreateScope().ServiceProvider;
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        if (!roleManager.Roles.Any() && !userManager.Users.Any())
        {
            IdentityRole adminsRole = new IdentityRole("Admins");
            await roleManager.CreateAsync(adminsRole);

            IdentityUser adminUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com"
            };
            await userManager.CreateAsync(adminUser, "admin");
            await userManager.AddToRoleAsync(await userManager.FindByNameAsync("admin"), "Admins");
            
            IdentityUser normalUser = new IdentityUser
            {
                UserName = "normal",
                Email = "normal@basic.com"
            };
            await userManager.CreateAsync(normalUser, "12345");
        }

        var admin = await userManager.FindByIdAsync("admin");
        if(admin != null && !(await userManager.IsInRoleAsync(admin, "Admins")))
        {
            await userManager.AddToRoleAsync(admin, "Admins");
            Console.WriteLine("added admin to admins.\n\n\n");
        }
    }
}