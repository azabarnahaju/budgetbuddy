namespace BudgetBuddy.Contracts.ModelRequest;

public record AccountCreateRequest(decimal Balance, string Name, string Type, string UserId);