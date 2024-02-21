using BudgetBuddy.Model;

namespace BudgetBuddy.Services.Authentication;

public interface IAuthenticationService
{
    bool Authenticate(AuthParams authParams);
}