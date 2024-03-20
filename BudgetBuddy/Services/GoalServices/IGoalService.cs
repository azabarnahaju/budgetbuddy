using BudgetBuddy.Model;

namespace BudgetBuddy.Services.GoalServices;

public interface IGoalService
{
    Task UpdateGoalProcess(Transaction transaction);
}