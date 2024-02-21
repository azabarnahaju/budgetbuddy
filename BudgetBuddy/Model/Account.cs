namespace BudgetBuddy.Model;

public record Account(int Id, DateTime Date, decimal Balance, string Name, string Type, int UserId, List<Transaction> Transactions);