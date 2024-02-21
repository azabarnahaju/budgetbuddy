namespace BudgetBuddy.Services.Authentication;

using Model;

public interface IAuthenticationService
{
    bool Authenticate(AuthParams authParams);
}