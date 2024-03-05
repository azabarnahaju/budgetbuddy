namespace BudgetBuddy.Services.Repositories.User;

using Model;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User> GetUser(string email);
    Task<User> GetUser(int id);
    Task<User> AddUser(Model.User user);
    Task<User> UpdateUser(Model.User user);
    Task DeleteUser(int id);
}