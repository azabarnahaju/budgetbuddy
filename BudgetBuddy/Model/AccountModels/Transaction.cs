using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Model.AccountModels;

public record Transaction(int Id, DateTime Date, decimal Amount, ExpenseCategory Tag, ExpenseType Type, int AccountId);