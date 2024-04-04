namespace BudgetBuddy.Services.Authentication;

public interface IAuthenticationSeeder
{
    void AddRoles();
    void AddAdmin();
}