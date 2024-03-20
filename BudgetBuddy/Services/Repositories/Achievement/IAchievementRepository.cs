using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;

namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public interface IAchievementRepository
{
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task<Achievement> GetAchievement(int id);
    Task<IEnumerable<Achievement>> AddAchievement(IEnumerable<AchievementCreateRequest> achievements);
    Task DeleteAchievement(int id);
    Task<Achievement> UpdateAchievement(AchievementUpdateRequest achievement);
}