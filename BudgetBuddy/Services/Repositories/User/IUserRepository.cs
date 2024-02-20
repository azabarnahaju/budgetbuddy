namespace BudgetBuddy.Services.Repositories.User;
using BudgetBuddy.Model;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUser(string email);
    User? GetUser(int id);
    bool AddUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(int id);
}