namespace BudgetBuddy.Model;

public class User
{
    public int Id { get; init; }
    public string Username { get; init; }
    public DateTime RegistrationDate { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public List<Achievement> Achievements { get; init; }
    
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        User other = (User)obj;

        return Id == other.Id &&
               RegistrationDate == other.RegistrationDate &&
               Username == other.Username &&
               Email == other.Email &&
               Password == other.Password;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, RegistrationDate, Username, Email, Password);
    }
}