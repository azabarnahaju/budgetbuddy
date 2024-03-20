using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Contracts.ModelRequest.CreateModels;

public record TransactionCreateRequest(string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);