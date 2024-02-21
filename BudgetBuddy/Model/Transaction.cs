namespace BudgetBuddy.Model;

using Enums;

public record Transaction(int Id, DateTime Date, decimal Amount, ExpenseCategory Tag, TransactionType Type, int AccountId);