using BudgetBuddy.Model.Enums;

namespace BudgetBuddy.Model;

public class GoalModel
{
    public int Id { get; init; }
    public int UserId { get; set; }
    public int AccountId { get; set; }
    public GoalType Type { get; set; }
    public decimal Target { get; set; }
    public decimal CurrentProgress { get; set; }
    public bool Completed { get; set; }
    public DateTime StartDate { get; set; }
}