namespace BudgetBuddy.Model;

using Enums;

public record Transaction(int Id, DateTime Date, decimal Amount, ExpenseCategory Tag, ExpenseType Type, int AccountId);