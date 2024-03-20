using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Contracts.ModelRequest.UpdateModels;

public record TransactionUpdateRequest(int Id, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);