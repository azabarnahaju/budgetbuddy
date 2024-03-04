namespace BudgetBuddy.Model;

public class User
{
    public int Id { get; init; }
    public string Username { get; init; }
    public DateTime RegistrationDate { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public List<Achievement> Achievements { get; init; }
}