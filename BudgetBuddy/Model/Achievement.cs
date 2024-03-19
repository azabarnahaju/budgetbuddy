namespace BudgetBuddy.Model;

public class Achievement
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public List<ApplicationUser> Users { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj is not Achievement || obj is null) return false;

        var other = (Achievement)obj;

        return other.Id == this.Id && other.Name == this.Name && other.Description == this.Description;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Description, Users);
    }
}