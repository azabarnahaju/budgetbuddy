using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Services.Authentication;

public interface ITokenService
{
    public string CreateToken(IdentityUser user, string role);
}