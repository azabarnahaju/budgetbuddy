using BudgetBuddy.Model.UserModels;

namespace BudgetBuddy.Services.Repositories.UserServices;

public interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User GetUser(string email);
    User GetUser(int id);
    User AddUser(User user);
    User UpdateUser(User user);
    void DeleteUser(int id);
}