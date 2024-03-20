using BudgetBuddy.Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Model;

public class Goal
{
    public int Id { get; init; }
    public string UserId { get; set; }
    public int AccountId { get; set; }
    public GoalType Type { get; set; }
    [Precision(14, 2)]
    public decimal Target { get; set; }
    [Precision(14, 2)]
    public decimal CurrentProgress { get; set; }
    public bool Completed { get; set; }
    public DateTime StartDate { get; set; }
}