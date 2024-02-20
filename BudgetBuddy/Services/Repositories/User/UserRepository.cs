namespace BudgetBuddy.Services.Repositories.User;
using BudgetBuddy.Model;
public class UserRepository : IUserRepository
{
    private IList<User> _users = new List<User>();
    
    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    public User? GetUser(string email)
    {
        return _users.All(user => user.Email != email) ? null : _users.First(user => user.Email == email);
    }

    public User? GetUser(int id)
    {
        return _users.All(user => user.Id != id) ? null : _users.First(user => user.Id == id);
    }

    public bool AddUser(User user)
    {
        if (_users.Any(u => u.Id == user.Id)) return false;

        _users.Add(user);
        return true;
    }

    public bool UpdateUser(User user)
    {
        var userToUpdate = _users.First(u => u.Id == user.Id);
        if (userToUpdate == user) return false;

        _users.Select(u =>
        {
            return user.Id == u.Id ? user : u;
        });
        return true;
    }

    public bool DeleteUser(int id)
    {
        if (_users.All(u => u.Id != id)) return false;

        _users = _users.Where(u => u.Id != id).ToList();
        return true;
    }
}