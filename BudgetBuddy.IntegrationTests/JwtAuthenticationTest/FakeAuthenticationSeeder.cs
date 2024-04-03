using BudgetBuddy.Model;
using BudgetBuddy.Services.Authentication;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.IntegrationTests.JwtAuthenticationTest;

public class FakeAuthenticationSeeder : IAuthenticationSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Dictionary<string, string> _adminInfo;
    
    public FakeAuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _adminInfo = new Dictionary<string, string>
        {
            {"adminEmail", "test@admin.com"},
            {"adminPassword", "test123"}
        };
    }
    
    public void AddRoles()
    {
        var tAdmin = CreateAdminRole(_roleManager);
        tAdmin.Wait();

        var tUser = CreateUserRole(_roleManager);
        tUser.Wait();
    }
    
    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await _userManager.FindByEmailAsync(_adminInfo["adminEmail"]);
        if (adminInDb is null)
        {
            var admin = new ApplicationUser { UserName = "admin", Email = _adminInfo["adminEmail"] };
            var adminCreated = await _userManager.CreateAsync(admin, _adminInfo["adminPassword"]);

            if (adminCreated.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
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