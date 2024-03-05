namespace BudgetBuddy.Services.Authentication;

using Model;

public interface IAuthenticationService
{
    Task<bool> Authenticate(AuthParams authParams);
}