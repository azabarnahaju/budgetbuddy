using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Model;

public class Account
{
    public int Id { get; init; }
    public DateTime Date { get; init; } 
    [Precision(14, 2)]
    public decimal Balance { get; init; } 
    public string Name { get; init; } 
    public string Type { get; init; } 
    public int UserId { get; init; } 
    public List<Transaction> Transactions { get; init; } 
}