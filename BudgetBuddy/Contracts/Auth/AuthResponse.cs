namespace BudgetBuddy.Contracts.Auth;

public record AuthResponse(string Email, string UserName, string Token);