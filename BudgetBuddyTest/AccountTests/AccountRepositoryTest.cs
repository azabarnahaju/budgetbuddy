// using BudgetBuddy.Data;
// using BudgetBuddy.Model;
// using BudgetBuddy.Services.Repositories.Account;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Diagnostics;
//
// namespace BudgetBuddyTest.AccountTests;
//
// public class AccountRepositoryTest
// {
//     private DbContextOptions<BudgetBuddyContext> _contextOptions;
//     private BudgetBuddyContext _context;
//     
//     [SetUp]
//     public void Setup()
//     {
//         _contextOptions = new DbContextOptionsBuilder<BudgetBuddyContext>()
//             .UseInMemoryDatabase("BloggingControllerTest")
//             .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
//             .Options;
//
//         _context = new BudgetBuddyContext(_contextOptions);
//
//         _context.Database.EnsureDeleted();
//         _context.Database.EnsureCreated();
//
//         _context.AddRange(_accounts);
//             
//         _context.SaveChanges();
//     }
//
//     [Test]
//     public async Task GetAccountSuccess()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var result = await accountRepository.GetAll();
//         
//         Assert.That(result.Count, Is.EqualTo(6));
//         Assert.That(result, Is.EquivalentTo(_accounts));
//     }
//     
//     [Test]
//     public async Task GetAccountByIdSuccess()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var result = await accountRepository.GetById(1);
//         
//         Assert.That(result, Is.EqualTo(_accounts[0]));
//     }
//     
//     [Test]
//     public async Task WrongIdFailToGetAccountById()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var accountId = 77; 
//         
//         Exception exception = null;
//         try
//         {
//             await accountRepository.GetById(accountId);
//         }
//         catch (KeyNotFoundException ex)
//         {
//             exception = ex;
//         }
//         Assert.That(exception, Is.Not.Null);
//         Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
//         Assert.That(exception.Message, Is.EqualTo("Account not found."));
//     }
//     
//     [Test]
//     public async Task CreateAccountSuccess()
//     {
//         var accountToCreate = new Account
//         {
//             Id = 152,
//             Date = new DateTime(2022, 02, 02),
//             Balance = 5.656m,
//             Name = "sample1",
//             Type = "simple1",
//             UserId = 1,
//             Transactions = new List<Transaction>()
//         };
//         var accountRepository = new AccountRepository(_context);
//         var result = await accountRepository.CreateAccount(accountToCreate);
//         
//         Assert.That(result, Is.EqualTo(accountToCreate));
//     }
//     
//     [Test]
//     public async Task SameIdFailToCreateAccount()
//     {
//         var accountToCreate = new Account
//         {
//             Id = 1,
//             Date = new DateTime(2022, 02, 02),
//             Balance = 5.656m,
//             Name = "sample1",
//             Type = "simple1",
//             UserId = 1,
//             Transactions = new List<Transaction>()
//         };
//         var accountRepository = new AccountRepository(_context);
//
//         Exception exception = null;
//         try
//         {
//             await accountRepository.CreateAccount(accountToCreate);
//         }
//         catch (Exception ex)
//         {
//             exception = ex;
//         }
//         Assert.That(exception, Is.Not.Null);
//         Assert.That(exception, Is.InstanceOf<Exception>());
//         Assert.That(exception.Message, Is.EqualTo("Cannot create new account."));
//     }
//     
//     [Test]
//     public async Task UpdateAccountSuccess()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var updatedAccount = new Account
//         {
//             Id = 1,
//             Date = new DateTime(2023, 02, 02),
//             Balance = 5.656m,
//             Name = "sample1",
//             Type = "simple2",
//             UserId = 5,
//             Transactions = new List<Transaction>()
//         };
//         var result = await accountRepository.UpdateAccount(updatedAccount);
//         
//         Assert.That(result, Is.EqualTo(updatedAccount));
//     }
//     
//     [Test]
//     public async Task WrongIdFailToUpdateAccount()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var updatedAccount = new Account
//         {
//             Id = 19,
//             Date = new DateTime(2023, 02, 02),
//             Balance = 5.656m,
//             Name = "sample1",
//             Type = "simple2",
//             UserId = 5,
//             Transactions = new List<Transaction>()
//         };
//         
//         Exception exception = null;
//         try
//         {
//             await accountRepository.UpdateAccount(updatedAccount);
//         }
//         catch (KeyNotFoundException ex)
//         {
//             exception = ex;
//         }
//         Assert.That(exception, Is.Not.Null);
//         Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
//         Assert.That(exception.Message, Is.EqualTo("Failed to update. Account not found."));
//     }
//
//     [Test]
//     public async Task DeleteAccountSuccess()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var accountId = 1;
//         
//         Exception exception = null;
//         try
//         {
//             await accountRepository.DeleteAccount(accountId);
//         }
//         catch (Exception ex)
//         {
//             exception = ex;
//         }
//
//         Assert.That(exception, Is.Null);
//     }
//     
//     [Test]
//     public async Task WrongIdFailToDeleteAccount()
//     {
//         var accountRepository = new AccountRepository(_context);
//         var accountId = 21;
//         
//         Exception exception = null;
//         try
//         {
//             await accountRepository.DeleteAccount(accountId);
//         }
//         catch (Exception ex)
//         {
//             exception = ex;
//         }
//
//         Assert.That(exception, Is.Not.Null);
//         Assert.That(exception, Is.InstanceOf<KeyNotFoundException>());
//         Assert.That(exception.Message, Is.EqualTo("Failed to delete. Account not found."));
//     }
//     
//     private readonly List<Account> _accounts = new()
//     {
//         new Account
//         {
//             Id = 1,
//             Date = new DateTime(2022, 02, 02),
//             Balance = 5.656m,
//             Name = "sample1",
//             Type = "simple1",
//             UserId = 1,
//             Transactions = new List<Transaction>()
//         },
//         new Account
//         {
//             Id = 2,
//             Date = new DateTime(2022, 03, 12),
//             Balance = 5.656m,
//             Name = "sample2",
//             Type = "simple2",
//             UserId = 2,
//             Transactions = new List<Transaction>()
//         },
//         new Account
//         {
//             Id = 3,
//             Date = new DateTime(2022, 04, 17),
//             Balance = 5.656m,
//             Name = "sample3",
//             Type = "simple3",
//             UserId = 1,
//             Transactions = new List<Transaction>()
//         },
//         new Account
//         {
//             Id = 4,
//             Date = new DateTime(2022, 05, 14),
//             Balance = 5.656m,
//             Name = "sample4",
//             Type = "simple4",
//             UserId = 2,
//             Transactions = new List<Transaction>()
//         },
//         new Account
//         {
//             Id = 5,
//             Date = new DateTime(2022, 06, 07),
//             Balance = 5.656m,
//             Name = "sample5",
//             Type = "simple5",
//             UserId = 1,
//             Transactions = new List<Transaction>()
//         },
//         new Account
//         {
//             Id = 6,
//             Date = new DateTime(2022, 07, 08),
//             Balance = 5.656m,
//             Name = "sample6",
//             Type = "simple6",
//             UserId = 1,
//             Transactions = new List<Transaction>()
//         }
//     };
// }
