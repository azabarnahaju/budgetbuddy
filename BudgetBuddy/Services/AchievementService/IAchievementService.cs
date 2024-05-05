using BudgetBuddy.Model;

namespace BudgetBuddy.Services.AchievementService;

public interface IAchievementService
{
    Task UpdateGoalAchievements(ApplicationUser user);
    Task UpdateRecordAchievements(ApplicationUser user);
    Task UpdateAccountAchievements(ApplicationUser user);
    Task UpdateTransactionAchievements(ApplicationUser user);
}