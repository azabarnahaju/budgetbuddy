using BudgetBuddy.Model;
using BudgetBuddy.Model.InputModels;

namespace BudgetBuddy.Services;

public interface IGoalService
{
    Task UpdateGoalProcess(TransactionInputModel transaction);
}