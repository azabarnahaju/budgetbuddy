using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Contracts.ModelResponse;

public record TransactionView(int Id, DateTime Date, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);
