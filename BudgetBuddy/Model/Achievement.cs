namespace BudgetBuddy.Model;

public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<int> Users { get; set; } 
}