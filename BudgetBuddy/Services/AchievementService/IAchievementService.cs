using BudgetBuddy.Model;

namespace BudgetBuddy.Services.AchievementService;

public interface IAchievementService
{
    Task UpdateAchievements(ApplicationUser user);
    Task BudgetMasterAchievement(ApplicationUser user, decimal budgetAmount);
    Task TransactionTrackerAchievement(ApplicationUser user);
    Task AccountAchievement(ApplicationUser user);
    Task SavingsAchievement(ApplicationUser user);
    // Task FirstGoalAchievement(ApplicationUser user);
    // Task GoalVisionary(ApplicationUser user);
    Task CategorizeAchievement(ApplicationUser user);
    
}