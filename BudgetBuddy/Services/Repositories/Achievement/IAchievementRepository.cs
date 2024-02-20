namespace BudgetBuddy.Services.Repositories.Achievement;
using BudgetBuddy.Model;

public interface IAchievementRepository
{
    IEnumerable<Achievement> GetAllAchievements();
    Achievement GetAchievement(int id);
    void AddAchievement(IEnumerable<Achievement> achievements);
    void AddAchievement(Achievement achievement);
    void DeleteAchievement(int id);
    void UpdateAchievement(Achievement achievement);
}