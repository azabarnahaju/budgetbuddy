namespace BudgetBuddy.Model;

public class User
{
    public int Id { get; init; }
    public string Username { get; init; }
    public DateTime RegistrationDate { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public List<Achievement> Achievements { get; init; }
    
    public override bool Equals(object? obj)
    {
        if (obj is not User || obj is null) return false;

        var other = (User)obj;

        return other.Id == this.Id && other.Username == this.Username && other.RegistrationDate == this.RegistrationDate && other.Email == this.Email 
               && other.Password == this.Password;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Username, RegistrationDate, Email, Password, Achievements);
    }
}