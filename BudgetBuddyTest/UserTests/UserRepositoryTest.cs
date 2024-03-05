using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BudgetBuddyTest.UserTests;

[TestFixture]
public class UserRepositoryTest
{
    private DbContextOptions<BudgetBuddyContext> _contextOptions;
    private BudgetBuddyContext _context;
    
    [SetUp]
    public void Setup()
    {
        _contextOptions = new DbContextOptionsBuilder<BudgetBuddyContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _context = new BudgetBuddyContext(_contextOptions);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _context.AddRange(_users);
            
        _context.SaveChanges();
    }

    [Test]
    public async Task GetUserSuccess()
    {
        var accountRepository = new UserRepository(_context);
        var result = await accountRepository.GetAllUsers();
        
        Assert.That(result.Count, Is.EqualTo(6));
        Assert.That(result, Is.EquivalentTo(_users));
    }
    
    [Test]
    public async Task GetUserByIdSuccess()
    {
        var userRepository = new UserRepository(_context);
        var result = await userRepository.GetUser(1);
        
        Assert.That(result, Is.EqualTo(_users[0]));
    }
    
    [Test]
    public async Task GetUserByEmailSuccess()
    {
        var userRepository = new UserRepository(_context);
        var result = await userRepository.GetUser("Email1");
        
        Assert.That(result, Is.EqualTo(_users[0]));
    }
    
    [Test]
    public async Task WrongIdFailToGetUserById()
    {
        var userRepository = new UserRepository(_context);
        var userId = 77; 
        
        Exception exception = null;
        try
        {
            await userRepository.GetUser(userId);
        }
        catch (KeyNotFoundException ex)
        {
            exception = ex;
        }
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
        Assert.That(exception.Message, Is.EqualTo("User not found."));
    }
    
    [Test]
    public async Task WrongIdFailToGetUserByName()
    {
        var userRepository = new UserRepository(_context);
        var userEmail = "Email12"; 
        
        Exception exception = null;
        try
        {
            await userRepository.GetUser(userEmail);
        }
        catch (KeyNotFoundException ex)
        {
            exception = ex;
        }
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
        Assert.That(exception.Message, Is.EqualTo("User not found."));
    }
    
    [Test]
    public async Task CreateUserSuccess()
    {
        var userToCreate = new User
        {
            Id = 152,
            RegistrationDate = new DateTime(2022, 02, 02),
            Username = "Admin152",
            Email = "Email152",
            Password = "password152",
            Achievements = new List<Achievement>()
        };
        var userRepository = new UserRepository(_context);
        var result = await userRepository.AddUser(userToCreate);
        
        Assert.That(result, Is.EqualTo(userToCreate));
    }
    
    [Test]
    public async Task SameIdFailToCreateAccount()
    {
        var userToCreate = new User
        {
            Id = 1,
            RegistrationDate = new DateTime(2022, 02, 02),
            Username = "Admin1",
            Email = "Email1",
            Password = "password1",
            Achievements = new List<Achievement>()
        };
        var userRepository = new UserRepository(_context);

        Exception exception = null;
        try
        {
            await userRepository.AddUser(userToCreate);
        }
        catch (Exception ex)
        {
            exception = ex;
        }
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception, Is.InstanceOf<Exception>());
        Assert.That(exception.Message, Is.EqualTo("Cannot create new user."));
    }
    
    [Test]
    public async Task UpdateAccountSuccess()
    {
        var userRepository = new UserRepository(_context);
        var updatedUser = new User
        {
            Id = 1,
            RegistrationDate = new DateTime(2022, 02, 02),
            Username = "Admin1",
            Email = "EditedEmail",
            Password = "password1",
            Achievements = new List<Achievement>()
        };
        var result = await userRepository.UpdateUser(updatedUser);
        
        Assert.That(result, Is.EqualTo(updatedUser));
    }
    
    [Test]
    public async Task WrongIdFailToUpdateAccount()
    {
        var userRepository = new UserRepository(_context);
        var updatedUser = new User
        {
            Id = 15,
            RegistrationDate = new DateTime(2022, 02, 02),
            Username = "Admin1",
            Email = "EditedEmail",
            Password = "password1",
            Achievements = new List<Achievement>()
        };
        
        Exception exception = null;
        try
        {
            await userRepository.UpdateUser(updatedUser);
        }
        catch (KeyNotFoundException ex)
        {
            exception = ex;
        }
        Assert.That(exception, Is.Not.Null);
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
        Assert.That(exception.Message, Is.EqualTo("Failed to update. User not found."));
    }

    [Test]
    public async Task DeleteAccountSuccess()
    {
        var userRepository = new UserRepository(_context);
        var userId = 1;
        
        Exception exception = null;
        try
        {
            await userRepository.DeleteUser(userId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.That(exception, Is.Null);
    }
    
    [Test]
    public async Task WrongIdFailToDeleteAccount()
    {
        var userRepository = new UserRepository(_context);
        var userId = 132;
        
        Exception exception = null;
        try
        {
            await userRepository.DeleteUser(userId);
        }
        catch (Exception ex)
        {
            exception = ex;
        }

        Assert.That(exception, Is.Not.Null);
        Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
        Assert.That(exception.Message, Is.EqualTo("Failed to delete. User not found."));
    }
    
    private readonly List<User> _users = new()
    {
        new User
        {
            Id = 1,
            Username = "Admin1",
            Email = "Email1",
            Password = "Password1",
            Achievements = new List<Achievement>(),
            RegistrationDate = new DateTime(2022, 03, 02)
        },
        new User
        {
            Id = 2,
            Username = "Admin2",
            Email = "Email2",
            Password = "Password2",
            Achievements = new List<Achievement>(),
            RegistrationDate = new DateTime(2022, 04, 02)
        },
        new User
        {
            Id = 3,
            Username = "Admin3",
            Email = "Email3",
            Password = "Password3",
            Achievements = new List<Achievement>(),
            RegistrationDate = new DateTime(2022, 05, 02)
        },
        new User
        {
            Id = 4,
            Username = "Admin4",
            Email = "Email4",
            Password = "Password4",
            Achievements = new List<Achievement>(),
            RegistrationDate = new DateTime(2022, 04, 02)
        },
        new User
        {
            Id = 5,
            Username = "Admin5",
            Email = "Email5",
            Password = "Password5",
            Achievements = new List<Achievement>(),
            RegistrationDate = new DateTime(2022, 05, 02)
        },
        new User
        {
            Id = 6,
            Username = "Admin6",
            Email = "Email6",
            Password = "Password6",
            Achievements = new List<Achievement>(),
            RegistrationDate = new DateTime(2022, 06, 02)
        }
    };
}