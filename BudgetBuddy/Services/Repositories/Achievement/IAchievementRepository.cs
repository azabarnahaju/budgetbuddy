namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public interface IAchievementRepository
{
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task<Achievement> GetAchievement(int id);
    Task<IEnumerable<Achievement>> AddAchievement(IEnumerable<Achievement> achievements);
    Task DeleteAchievement(int id);
    Task<Achievement> UpdateAchievement(Achievement achievement);
}