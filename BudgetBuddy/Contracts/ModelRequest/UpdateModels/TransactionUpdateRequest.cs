using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Contracts.ModelRequest.UpdateModels;

public record TransactionUpdateRequest(int Id, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);