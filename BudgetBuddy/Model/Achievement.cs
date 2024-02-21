namespace BudgetBuddy.Model;

public class Achievement
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<int> Users { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Achievement || obj is null) return false;

        var other = (Achievement)obj;

        return other.Id == this.Id && other.Name == this.Name && other.Description == this.Description &&
               other.Users == this.Users;
    }
}