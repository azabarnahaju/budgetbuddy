namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public interface IAchievementRepository
{
    IEnumerable<Achievement> GetAllAchievements();
    Achievement GetAchievement(int id);
    IEnumerable<Achievement> AddAchievement(IEnumerable<Achievement> achievements);
    void DeleteAchievement(int id);
    Achievement UpdateAchievement(Achievement achievement);
}