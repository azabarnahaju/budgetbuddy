namespace BudgetBuddy.Model.CreateModels;

using Microsoft.EntityFrameworkCore;

public class AccountInputModel
{
    public DateTime Date { get; init; } 
    [Precision(14, 2)]
    public decimal Balance { get; init; } 
    public string Name { get; init; } 
    public string Type { get; init; } 
    public string UserId { get; init; }
}