namespace BudgetBuddy.Model.CreateModels;

using Enums;

public class GoalInputModel
{
    public int UserId { get; set; }
    public int AccountId { get; set; }
    public GoalType Type { get; set; }
    public decimal Target { get; set; }
    public decimal CurrentProgress { get; set; }
    public bool Completed { get; set; }
    public DateTime StartDate { get; set; }
}