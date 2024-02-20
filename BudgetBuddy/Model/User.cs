namespace BudgetBuddy.Model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}