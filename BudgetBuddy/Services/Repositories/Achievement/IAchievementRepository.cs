using BudgetBuddy.Model.CreateModels;
using BudgetBuddy.Model.UpdateModels;

namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public interface IAchievementRepository
{
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task<Achievement> GetAchievement(int id);
    Task<Achievement> AddAchievement(AchievementInputModel achievements);
    Task DeleteAchievement(int id);
    Task<Achievement> UpdateAchievement(AchievementUpdateModel achievement);
}