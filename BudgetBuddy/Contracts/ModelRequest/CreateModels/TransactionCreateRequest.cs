using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.TransactionEnums;

namespace BudgetBuddy.Contracts.ModelRequest.CreateModels;

public record TransactionCreateRequest(DateTime Date, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);