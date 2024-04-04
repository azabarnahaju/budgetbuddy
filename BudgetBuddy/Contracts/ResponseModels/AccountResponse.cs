using BudgetBuddy.Model;

namespace BudgetBuddy.Contracts.ResponseModels;

public record AccountResponse(string Message, List<Account> Data);