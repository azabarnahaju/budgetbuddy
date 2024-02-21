namespace BudgetBuddy.Services.Repositories.User;

using Model;

public class UserRepository : IUserRepository
{
    private IList<User> _users = new List<User>();
    
    public IEnumerable<User> GetAllUsers()
    {
        return _users;
    }

    public User GetUser(string email)
    {
        return _users.All(user => user.Email != email) ? throw new InvalidDataException("User not found") : _users.First(user => user.Email == email);
    }

    public User GetUser(int id)
    {
        return _users.All(user => user.Id != id) ? throw new InvalidDataException("User not found") : _users.First(user => user.Id == id);
    }

    public User AddUser(User user)
    {
        if (_users.Any(u => u.Id == user.Id)) throw new Exception("User already exists.");

        _users.Add(user);
        return user;
    }

    public User UpdateUser(User user)
    {
        if (_users.FirstOrDefault(u => u.Id == user.Id) is null) throw new Exception("User not found.");

        _users = _users.Select(u => user.Id == u.Id ? user : u).ToList();
        return _users.First(u => u.Id == user.Id);
    }

    public void DeleteUser(int id)
    {
        if (_users.Count == 0 || _users.All(u => u.Id != id)) throw new Exception("User is not found.");

        _users = _users.Where(u => u.Id != id).ToList();
    }
}