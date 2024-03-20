namespace BudgetBuddy.Services.Repositories.Goal;

using Contracts.ModelRequest.CreateModels;
using Model;

public interface IGoalRepository
{
    Task<Goal[]> GetAllGoalsByAccountId(int accountId, bool? completed = null);
    Task<Goal> CreateGoal(GoalCreateRequest goal);
    Task<Goal> UpdateGoal(Goal goal);
    Task DeleteGoal(int id);
}