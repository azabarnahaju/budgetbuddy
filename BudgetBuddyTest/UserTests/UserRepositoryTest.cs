using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.User;

namespace BudgetBuddyTest.UserTests;

[TestFixture]
public class UserRepositoryTest
{
    [Test]
    public void GetAllUsers_ReturnsEmptyListWhenNoUsers()
    {
        var repository = new UserRepository(new List<User>());
        var users = repository.GetAllUsers();
        
        Assert.That(users.Count(), Is.EqualTo(0));
    }

    [Test]
    public void GetAllUsers_ReturnsCorrectUsers()
    {
        var userList = new List<User> { new User{ Id = 1 }, new User{ Id = 2 }, new User{ Id = 3 } };
        var repository = new UserRepository(userList);
        var actualResult = repository.GetAllUsers();
        
        Assert.Multiple(() =>
        {
            Assert.That(actualResult, Is.EquivalentTo(userList));
            Assert.That(actualResult.Count(), Is.EqualTo(userList.Count));
        });
    }

    [Test]
    public void GetUserByEmail_ThrowsExceptionWhenUserNotExist()
    {
        var userList = new List<User> { new User { Id = 1, Email = "AB" }, new User { Id = 2, Email = "CD" } };
        var repository = new UserRepository(userList);

        Assert.Throws<InvalidDataException>(() => repository.GetUser("EF"));
    }
    
    [Test]
    public void GetUserByEmail_ReturnsCorrectUser()
    {
        var user1 = new User { Id = 1, Email = "AB" };
        var user2 = new User { Id = 2, Email = "CD" };
        
        var userList = new List<User> { user1, user2 };
        var repository = new UserRepository(userList);
        
        var actualResult = repository.GetUser("AB");
        
        Assert.That(actualResult, Is.EqualTo(user1));
    }

    [Test]
    public void GetUserById_ThrowsExceptionWhenUserNotExist()
    {
        var userList = new List<User> { new User { Id = 1 }, new User { Id = 2, } };
        var repository = new UserRepository(userList);

        Assert.Throws<InvalidDataException>(() => repository.GetUser(3));
    }
    
    [Test]
    public void GetUserById_ReturnsCorrectUser()
    {
        var user1 = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        
        var userList = new List<User> { user1, user2 };
        var repository = new UserRepository(userList);
        
        var actualResult = repository.GetUser(1);
        
        Assert.That(actualResult, Is.EqualTo(user1));
    }

    [Test]
    public void AddUser_ThrowsExceptionWhenUserAlreadyExists()
    {
        var userList = new List<User> { new User { Id = 1 }, new User { Id = 2, } };
        var repository = new UserRepository(userList);

        Assert.Throws<Exception>(() => repository.AddUser(new User { Id = 1 }));
    }

    [Test]
    public void AddUser_SuccessfullyAddsUserWhenUserNotExistYet()
    {
        var userList = new User[] { new User { Id = 1 }, new User { Id = 2, } };
        var repository = new UserRepository(userList.ToList());

        var actualResult = repository.AddUser(new User { Id = 3 });
        var currentRepository = repository.GetAllUsers();
        
        Assert.Multiple(() =>
        {
            Assert.That(currentRepository.Count(), Is.EqualTo(userList.Count() + 1));
            Assert.That(currentRepository.Contains(actualResult));
        });
    }

    [Test]
    public void UpdateUser_ThrowsExceptionWhenUserNotExist()
    {
        var userList = new User[] { new User { Id = 1 }, new User { Id = 2 } };
        var repository = new UserRepository(userList.ToList());

        Assert.Throws<Exception>(() => repository.UpdateUser(new User { Id = 10, Username = "ABCD" }));
    }
    
    [Test]
    public void UpdateUser_UpdatesCorrectUser()
    {
        var user1 = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        var userList = new User[] { user1, user2 };
        var repository = new UserRepository(userList.ToList());

        var userWithUpdate = new User { Id = 2, Email = "ABCD" };
        var actualResult = repository.UpdateUser(userWithUpdate);

        var updatedUser = repository.GetUser(2);
        
        Assert.Multiple(() =>
        {
            Assert.That(actualResult, Does.Not.EqualTo(user2));
            Assert.That(actualResult, Is.EqualTo(updatedUser));
        });
    }

    [Test]
    public void DeleteUser_ThrowsExceptionWhenUserNotExist()
    {
        var userList = new User[] { new User { Id = 1 }, new User { Id = 2 } };
        var repository = new UserRepository(userList.ToList());

        Assert.Throws<Exception>(() => repository.DeleteUser(5));
    }
    
    [Test]
    public void DeleteUser_SuccessfullyDeletesUser()
    {
        var user1 = new User { Id = 1 };
        var user2 = new User { Id = 2 };
        var userList = new User[] { user1, user2 };
        var repository = new UserRepository(userList.ToList());

        repository.DeleteUser(1);

        var currentRepository = repository.GetAllUsers();
        
        Assert.Multiple(() =>
        {
            Assert.That(currentRepository, Does.Not.Contain(user1));
            Assert.That(currentRepository.Count(), Is.EqualTo(userList.Length - 1));
        });
    }
}