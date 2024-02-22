using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Account;

namespace BudgetBuddyTest.AccountTests;

public class AccountRepositoryTest
{
    [TestCaseSource(nameof(_testCasesForGetAll))]
    public void TestIfGetAllMethodGetCorrectNumberOfResults(List<Account> accounts, int expectedCount)
    {
        var repository = new AccountRepository(accounts);
        var result = repository.GetAll().Count;
        Assert.That(result, Is.EqualTo(expectedCount));
    }
    
    [TestCaseSource(nameof(_testCasesForGetAll))]
    public void TestIfGetAllMethodGetCorrectList(List<Account> accounts, int expectedCount)
    {
        var repository = new AccountRepository(accounts);
        var result = repository.GetAll();
        Assert.That(result, Is.EquivalentTo(accounts));
    }
    
    [TestCaseSource(nameof(_testCasesForGetById))]
    public void TestIfGetByIdMethodGetCorrectAccount(List<Account> accounts, int id, Account expectedAccount)
    {
        var repository = new AccountRepository(accounts);
        var result = repository.GetById(id);
        Assert.That(result, Is.EqualTo(expectedAccount));
    }

    [TestCaseSource(nameof(_testCasesForCreateAccount))]
    public void TestIfCreateAccountIsSuccessful(Account account)
    {
        var repository = new AccountRepository(new List<Account>());
        repository.CreateAccount(account);
        var result = repository.GetById(account.Id);
        Assert.That(account, Is.EqualTo(result));
    }
    
    [TestCaseSource(nameof(_testCasesForUpdateAccount))]
    public void TestIfUpdateAccountIsSuccessful(List<Account> accounts, Account account)
    {
        var repository = new AccountRepository(accounts);
        repository.UpdateAccount(account);
        var result = repository.GetById(account.Id);
        Assert.That(account, Is.EqualTo(result));
    }
    
    [TestCaseSource(nameof(_testCasesForDeleteAccount))]
    public void TestIfDeleteAccountIsSuccessful(List<Account> accounts, int id, List<Account> expectedResult)
    {
        var repository = new AccountRepository(accounts);
        repository.DeleteAccount(id);
        var result = repository.GetAll();
        Assert.That(result, Is.EquivalentTo(expectedResult));
    }

    private static readonly List<Account> Accounts = new()
    {
        new(1, new DateTime(2022, 02, 02), 5.656m, "sample1", "simple1", 1, new List<Transaction>()),
        new(2, new DateTime(2022, 03, 02), 54.656m, "sample1", "simple2", 12, new List<Transaction>()),
        new(3, new DateTime(2022, 04, 02), 3.656m, "sample2", "simple3", 2, new List<Transaction>()),
        new(4, new DateTime(2022, 05, 02), 112.656m, "sample3", "simple4", 1, new List<Transaction>()),
        new(5, new DateTime(2022, 06, 02), 53.656m, "sample4", "simple5", 3, new List<Transaction>()),
        new(6, new DateTime(2022, 07, 02), 756.656m, "sample5", "simple6", 1, new List<Transaction>())
    };
    
    private static readonly List<Account> AccountsForId = new()
    {
        new(1, new DateTime(2022, 02, 02), 5.656m, "sample1", "simple1", 1, new List<Transaction>()),
        new(2, new DateTime(2022, 03, 02), 54.656m, "sample1", "simple2", 12, new List<Transaction>()),
        new(3, new DateTime(2022, 04, 02), 3.656m, "sample2", "simple3", 2, new List<Transaction>()),
        new(4, new DateTime(2022, 05, 02), 112.656m, "sample3", "simple4", 1, new List<Transaction>()),
        new(5, new DateTime(2022, 06, 02), 53.656m, "sample4", "simple5", 3, new List<Transaction>()),
        new(6, new DateTime(2022, 07, 02), 756.656m, "sample5", "simple6", 1, new List<Transaction>())
    };
    
    private static readonly List<Account> UpdatedAccounts = new()
    {
        new(1, new DateTime(2023, 04, 03), 55.656m, "sample3", "simple6", 1, new List<Transaction>()),
        new(2, new DateTime(2023, 01, 03), 554.656m, "sample2", "simple5", 12, new List<Transaction>()),
        new(3, new DateTime(2023, 05, 03), 2.656m, "sample4", "simple1", 2, new List<Transaction>()),
        new(4, new DateTime(2023, 06, 03), 42.656m, "sample6", "simple2", 1, new List<Transaction>()),
        new(5, new DateTime(2023, 07, 03), 54.656m, "sample7", "simple45", 3, new List<Transaction>()),
        new(6, new DateTime(2023, 08, 03), 74.656m, "sample1", "simple5", 1, new List<Transaction>())
    };
    
    private static object[] _testCasesForGetAll =
    {
        new object[] { new List<Account>
        {
            new(1, new DateTime(2022, 02, 02), 5.656m, "sample1", "simple1", 1, new List<Transaction>()),
            new(2, new DateTime(2022, 03, 02), 54.656m, "sample1", "simple2", 12, new List<Transaction>()),
            new(3, new DateTime(2022, 04, 02), 3.656m, "sample2", "simple3", 2, new List<Transaction>()),
            new(4, new DateTime(2022, 05, 02), 112.656m, "sample3", "simple4", 1, new List<Transaction>()),
            new(5, new DateTime(2022, 06, 02), 53.656m, "sample4", "simple5", 3, new List<Transaction>()),
            new(6, new DateTime(2022, 07, 02), 756.656m, "sample5", "simple6", 1, new List<Transaction>())
        }, 6 }
    };
    
    private static object[] _testCasesForGetById =
    {
        new object[] { AccountsForId, 1, AccountsForId[0] },
        new object[] { AccountsForId, 2, AccountsForId[1] },
        new object[] { AccountsForId, 6, AccountsForId[5] },
        new object[] { AccountsForId, 3, AccountsForId[2] },
        new object[] { AccountsForId, 4, AccountsForId[3] },
        new object[] { AccountsForId, 5, AccountsForId[4] }
    };
    
    private static object[] _testCasesForCreateAccount =
    {
        new object[] { Accounts[0] },
        new object[] { Accounts[1] },
        new object[] { Accounts[5] },
        new object[] { Accounts[2] },
        new object[] { Accounts[3] },
        new object[] { Accounts[4] }
    };
    
    private static object[] _testCasesForUpdateAccount =
    {
        new object[] { AccountsForId, UpdatedAccounts[0] },
        new object[] { AccountsForId, UpdatedAccounts[1] },
        new object[] { AccountsForId, UpdatedAccounts[2] },
        new object[] { AccountsForId, UpdatedAccounts[3] },
        new object[] { AccountsForId, UpdatedAccounts[4] },
        new object[] { AccountsForId, UpdatedAccounts[5] }
    };
    
    private static object[] _testCasesForDeleteAccount =
    {
        new object[] { Accounts, 1, Accounts.Where(acc => acc.Id != 1).ToList() }
    };
}