namespace BudgetBuddy.Model;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}