using System.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Account;

namespace BudgetBuddyTest.AccountTests;

public class AccountRepositoryTest
{
    [Test]
    public void GetAll_ReturnsCorrectNumberOfAccounts()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var repository = new AccountRepository(accounts);
        
        // Act
        var result = repository.GetAll();
        
        // Assert
        Assert.That(result.Count, Is.EqualTo(accounts.Count));
    }
    
    [Test]
    public void GetAll_ReturnsAllAccounts()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var repository = new AccountRepository(accounts);
        
        // Act
        var result = repository.GetAll();
        
        // Assert
        Assert.That(result, Is.EquivalentTo(accounts));
    }
    
    [Test]
    public void GetById_ReturnsCorrectAccount()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var repository = new AccountRepository(accounts);
        var expectedAccount = accounts.First();
        
        // Act
        var result = repository.GetById(expectedAccount.Id);
        
        // Assert
        Assert.That(result, Is.EqualTo(expectedAccount));
    }
    
    [Test]
    public void GetById_InvalidIdThrowsException()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var repository = new AccountRepository(accounts);
    
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => repository.GetById(7));
    }

    [Test]
    public void CreateAccount_Succeeds()
    {
        // Arrange
        var account = new Account(7, DateTime.Now, 100m, "sample", "simple", 1, new List<Transaction>());
        var repository = new AccountRepository(new List<Account>());
        
        // Act
        repository.CreateAccount(account);
        var result = repository.GetById(account.Id);
        
        // Assert
        Assert.That(result, Is.EqualTo(account));
    }
    
    [Test]
    public void CreateAccount_WithDuplicateId_ThrowsException()
    {
        // Arrange
        var accountId = 5;
        var account = new Account(accountId, DateTime.Now, 100m, "sample", "simple", 1, new List<Transaction>());
        var repository = new AccountRepository(GetTestAccounts());

        // Act & Assert
        Assert.Throws<InvalidConstraintException>(() => repository.CreateAccount(account));
    }
    
    [Test]
    public void UpdateAccount_Succeeds()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var updatedAccount = new Account(1, DateTime.Now, 100m, "sample", "simple", 1, new List<Transaction>());
        var repository = new AccountRepository(accounts);
        
        // Act
        repository.UpdateAccount(updatedAccount);
        var result = repository.GetById(updatedAccount.Id);
        
        // Assert
        Assert.That(result, Is.EqualTo(updatedAccount));
    }
    
    [Test]
    public void UpdateAccount_FailsInvalidId()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var updatedAccount = new Account(7, DateTime.Now, 100m, "sample", "simple", 1, new List<Transaction>());
        var repository = new AccountRepository(accounts);
        
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => repository.UpdateAccount(updatedAccount));
    }
    
    [Test]
    public void DeleteAccount_Succeeds()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var idToDelete = 1;
        var expectedAccounts = accounts.Where(acc => acc.Id != idToDelete).ToList();
        var repository = new AccountRepository(accounts);
        
        // Act
        repository.DeleteAccount(idToDelete);
        var result = repository.GetAll();
        
        // Assert
        Assert.That(result, Is.EquivalentTo(expectedAccounts));
    }
    
    [Test]
    public void DeleteAccount_FailsInvalidId()
    {
        // Arrange
        var accounts = GetTestAccounts();
        var idToDelete = 7;
        var expectedAccounts = accounts.Where(acc => acc.Id != idToDelete).ToList();
        var repository = new AccountRepository(accounts);
        
        // Act & Assert
        Assert.Throws<KeyNotFoundException>(() => repository.DeleteAccount(idToDelete));
    }

    private static List<Account> GetTestAccounts()
    {
        return new List<Account>
        {
            new(1, new DateTime(2022, 02, 02), 5.656m, "sample1", "simple1", 1, new List<Transaction>()),
            new(2, new DateTime(2022, 03, 02), 54.656m, "sample1", "simple2", 12, new List<Transaction>()),
            new(3, new DateTime(2022, 04, 02), 3.656m, "sample2", "simple3", 2, new List<Transaction>()),
            new(4, new DateTime(2022, 05, 02), 112.656m, "sample3", "simple4", 1, new List<Transaction>()),
            new(5, new DateTime(2022, 06, 02), 53.656m, "sample4", "simple5", 3, new List<Transaction>()),
            new(6, new DateTime(2022, 07, 02), 756.656m, "sample5", "simple6", 1, new List<Transaction>())
        };
    }
}

