using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Contracts.ModelRequest;

public record TransactionRequest(DateTime Date, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);