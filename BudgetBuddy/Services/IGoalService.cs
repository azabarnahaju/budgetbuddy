using BudgetBuddy.Model;

namespace BudgetBuddy.Services;

public interface IGoalService
{
    Task UpdateGoalProcess(Transaction transaction);
}