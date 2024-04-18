using BudgetBuddy.Model;

namespace BudgetBuddy.Services.Repositories.User;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserById(string id);
    Task<ApplicationUser?> GetUserByAccountId(int accountId);
}