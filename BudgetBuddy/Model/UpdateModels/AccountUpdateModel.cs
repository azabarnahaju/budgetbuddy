namespace BudgetBuddy.Model.UpdateModels;

using Enums;
using Microsoft.EntityFrameworkCore;



public class AccountUpdateModel
{
    public int Id { get; init; }
    public DateTime Date { get; init; } 
    [Precision(14, 2)]
    public decimal Balance { get; init; } 
    public string Name { get; init; } 
    public string Type { get; init; } 
    public string UserId { get; init; }
}