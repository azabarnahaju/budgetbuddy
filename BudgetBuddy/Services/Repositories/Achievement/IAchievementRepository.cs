using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;

namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public interface IAchievementRepository
{
    Task<IEnumerable<Achievement>> GetAllAchievements();
    Task<Achievement> GetAchievement(int id);
    Task<IEnumerable<Achievement>> GetAllAchievementsByUserId(string userId);
    Task<IEnumerable<Achievement>> GetAchievementsByObjective(AchievementObjectiveType objective);
    Task<IEnumerable<Achievement>> AddAchievement(IEnumerable<AchievementCreateRequest> achievements);
    Task DeleteAchievement(int id);
    Task<Achievement> UpdateAchievement(AchievementUpdateRequest achievement);
}