using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Model;

using Enums;

public record Transaction
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Name { get; init; }
    [Precision(14, 2)]
    public decimal Amount { get; init; }
    public TransactionCategoryTag Tag { get; init; }
    public TransactionType Type { get; init; }
    public Account Account { get; init; } = null!;
    public int AccountId { get; init; }
};

