using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Model.RequestModels;

public record TransactionRequest(DateTime Date, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);