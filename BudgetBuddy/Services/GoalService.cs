using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Repositories.Goal;

namespace BudgetBuddy.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;
    
    public GoalService(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task UpdateGoalProcess(Transaction transaction)
    {
        var allGoalsInProgress = await _goalRepository.GetAllGoalsByAccountId(transaction.AccountId, false);
        if (allGoalsInProgress.Length > 0)
        {
            foreach (var goal in allGoalsInProgress)
            {
                await HandleGoalChange(goal, transaction);
            }
        }
    }

    private async Task HandleGoalChange(GoalModel goal, Transaction transaction)
    {
        if (goal.Type == GoalType.Spending && transaction.Type == TransactionType.Expense)
        {
            goal.CurrentProgress += transaction.Amount;
            CheckIfGoalCompleted(goal);
            await _goalRepository.UpdateGoal(goal);
        }

        if (goal.Type == GoalType.Income && transaction.Type == TransactionType.Income)
        {
            goal.CurrentProgress += transaction.Amount;
            CheckIfGoalCompleted(goal);
            await _goalRepository.UpdateGoal(goal);
        }

        if (goal.Type == GoalType.CharitableGiving && transaction.Tag == TransactionCategoryTag.CharitableDonations)
        {
            goal.CurrentProgress += transaction.Amount;
            CheckIfGoalCompleted(goal);
            await _goalRepository.UpdateGoal(goal);
        }
        
        if (goal.Type == GoalType.DebtRepayment && transaction.Tag == TransactionCategoryTag.DeptRepayment)
        {
            goal.CurrentProgress += transaction.Amount;
            CheckIfGoalCompleted(goal);
            await _goalRepository.UpdateGoal(goal);
        }
        
        if (goal.Type == GoalType.Investment && transaction.Tag == TransactionCategoryTag.Investment)
        {
            goal.CurrentProgress += transaction.Amount;
            CheckIfGoalCompleted(goal);
            await _goalRepository.UpdateGoal(goal);
        }
    }

    private void CheckIfGoalCompleted(GoalModel goal)
    {
        if (goal.CurrentProgress >= goal.Target)
        {
            goal.Completed = true;
        }
    }
}