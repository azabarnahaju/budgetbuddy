using BudgetBuddy.Services.Authentication;

namespace BudgetBuddy.IntegrationTests.JwtAuthenticationTest;

public class FakeAuthenticationSeeder : IAuthenticationSeeder
{
    public void AddRoles()
    {
        return;
    }

    public void AddAdmin()
    {
        return;
    }
}