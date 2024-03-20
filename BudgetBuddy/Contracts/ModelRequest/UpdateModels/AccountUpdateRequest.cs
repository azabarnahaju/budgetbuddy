namespace BudgetBuddy.Contracts.ModelRequest.UpdateModels;

public record AccountUpdateRequest(int Id, decimal Balance, string Name, string Type, string UserId);