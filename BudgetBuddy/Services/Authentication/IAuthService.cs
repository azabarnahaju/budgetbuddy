namespace BudgetBuddy.Services.Authentication;

using Model;

public interface IAuthenticationService
{
    Task<AuthResult> RegisterAsync(string email, string username, string password);
}