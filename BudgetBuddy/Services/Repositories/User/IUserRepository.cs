namespace BudgetBuddy.Services.Repositories.User;

using Model;

public interface IUserRepository
{
    Task<ApplicationUser?> GetUserById(string id);
    Task<ApplicationUser?> GetUserByAccountId(int accountId);
    Task AddAchievementToUser(string userId, Achievement achievement);
}