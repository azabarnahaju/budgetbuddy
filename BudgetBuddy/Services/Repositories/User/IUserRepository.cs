namespace BudgetBuddy.Services.Repositories.User;

using Model;

public interface IUserRepository
{
    IEnumerable<Model.User> GetAllUsers();
    User GetUser(string email);
    User GetUser(int id);
    User AddUser(Model.User user);
    User UpdateUser(Model.User user);
    void DeleteUser(int id);
}