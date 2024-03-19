using BudgetBuddy.Model;

namespace BudgetBuddy.Services.Repositories.Goal;

public interface IGoalRepository
{
    Task<GoalModel[]> GetAllGoalsByUserId(int userId);
    Task<GoalModel> CreateGoal(GoalModel goal);
    Task<GoalModel> UpdateGoal(GoalModel goal);
    Task DeleteGoal(int id);
}