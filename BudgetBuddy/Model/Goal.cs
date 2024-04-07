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
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        Goal other = (Goal)obj;

        return Id == other.Id &&
               UserId == other.UserId &&
               AccountId == other.AccountId &&
               Type == other.Type &&
               Target == other.Target &&
               CurrentProgress == other.CurrentProgress &&
               Completed == other.Completed;
    }
}