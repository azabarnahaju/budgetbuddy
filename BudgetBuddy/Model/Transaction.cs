namespace BudgetBuddy.Model;

using Enums;

public record Transaction(int Id, DateTime Date, string Name, decimal Amount, TransactionCategoryTag Tag, TransactionType Type, int AccountId);

