using BudgetBuddy.Model;

namespace BudgetBuddy.Services.AchievementService;

public interface IAchievementService
{
    Task UpdateAccountAchievements(ApplicationUser user);
    Task UpdateTransactionAchievements(ApplicationUser user);
    Task UpdateGoalAchievements(ApplicationUser user);
}