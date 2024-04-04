using BudgetBuddy.Model;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Services.Authentication;

public class AuthenticationSeeder : IAuthenticationSeeder
{
    private RoleManager<IdentityRole> roleManager;
    private UserManager<ApplicationUser> userManager;
    private Dictionary<string, string> adminInfo;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, Dictionary<string, string> adminInfo)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.adminInfo = adminInfo;
    }
    
    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(roleManager);
        tAdmin.Wait();

        var tUser = CreateUserRole(roleManager);
        tUser.Wait();
    }
    
    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync(adminInfo["adminEmail"]);
        if (adminInDb is null)
        {
            var admin = new ApplicationUser { UserName = "admin", Email = adminInfo["adminEmail"] };
            var adminCreated = await userManager.CreateAsync(admin, adminInfo["adminPassword"]);

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
    
    private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("User"));
    }
}