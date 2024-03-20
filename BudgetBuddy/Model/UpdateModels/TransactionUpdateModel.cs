namespace BudgetBuddy.Model.UpdateModels;

using Enums;
using Microsoft.EntityFrameworkCore;

public class TransactionUpdateModel
{
    public int Id { get; init; }
    public DateTime Date { get; init; }
    public string Name { get; init; }
    [Precision(14, 2)]
    public decimal Amount { get; init; }
    public TransactionCategoryTag Tag { get; init; }
    public TransactionType Type { get; init; }
    public int AccountId { get; init; }
}