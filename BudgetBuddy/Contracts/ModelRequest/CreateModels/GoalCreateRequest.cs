using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Contracts.ModelRequest.CreateModels;

public record GoalCreateRequest(string UserId, int AccountId, GoalType Type, decimal Target, decimal CurrentProgress, bool Completed, DateTime StartDate);