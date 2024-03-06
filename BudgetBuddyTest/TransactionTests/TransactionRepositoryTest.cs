using BudgetBuddy.Services.Repositories.Achievement;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BudgetBuddyTest.TransactionTests;

using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Repositories.Transaction;
using Microsoft.Extensions.Logging;
using Moq;
using BudgetBuddy.Data;
using Microsoft.EntityFrameworkCore;

[TestFixture]
public class TransactionRepositoryTest
{
    private DbContextOptions<BudgetBuddyContext> _contextOptions;
    private BudgetBuddyContext _context;
    private ITransactionRepository _repository;
    
    [SetUp]
    public void SetUp()
    {
        _contextOptions = new DbContextOptionsBuilder<BudgetBuddyContext>()
            .UseInMemoryDatabase("Achievements")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        _context = new BudgetBuddyContext(_contextOptions);
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.SaveChanges();
        
        _repository = new TransactionRepository(_context);
    }
    
    [Test]
    public async Task AddTransactionReturnsTransaction()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        var result = _repository.GetTransaction(1);
        
        Assert.That(transaction, Is.EqualTo(result.Result));
        
    }
    
    [Test]
    public async Task AddTransactionThrowsExceptionIfTransactionExists()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        var exception = Assert.Throws<Exception>(() => _repository.AddTransaction(transaction));
        Assert.That(exception?.Message, Is.EqualTo("Transaction already exists."));
    }
    
    [Test]
    public async Task GetAllTransactionsReturnsTransactions()
    {
        var transaction1 = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        await _context.AddAsync(transaction1);
        
        var transaction2 = new Transaction
        {
            Id = 2,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        await _context.AddAsync(transaction2);
        await _context.SaveChangesAsync();
        
        var result = await _repository.GetAllTransactions();
        
        Assert.That(result.ToList().Count(), Is.EqualTo(2));
        Assert.That(new List<Transaction> { transaction1, transaction2 }, Is.EqualTo(result.ToList()));
    }
    
    [Test]
    public async Task GetTransactionReturnsTransaction()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        var result = await _repository.GetTransaction(1);
        
        Assert.That(transaction, Is.EqualTo(result));
    }
    
    [Test]
    public async Task GetTransactionThrowsExceptionIfTransactionNotFound()
    {
        var exception = Assert.ThrowsAsync<Exception>(async () => await _repository.GetTransaction(1));
        
        Assert.That(exception?.Message, Is.EqualTo("Transaction not found."));
    }

    
    [Test]
    public async Task UpdateTransactionReturnsTransaction()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        var result = await _repository.UpdateTransaction(transaction);
        
        Assert.That(transaction, Is.EqualTo(result));
    }

    [Test]
    public async Task UpdateTransactionThrowsExceptionIfTransactionNotFound()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        var transactionToUpdate = new Transaction
        {
            Id = 2,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 2
        };
        var aggregateException = Assert.Throws<AggregateException>(() => _repository.UpdateTransaction(transactionToUpdate).Wait());
        var exception = aggregateException.InnerException;
        Assert.That(exception, Is.InstanceOf<Exception>());
        Assert.That(exception.Message, Is.EqualTo("Transaction not found."));
    }

    
    [Test]
    public void DeleteTransactionRemovesTransaction()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
         _context.Add(transaction);
         _context.SaveChanges();
        
        _repository.DeleteTransaction(1);
    
        Assert.IsEmpty(_context.Transactions.ToList());
    }
    
    [Test]
    public void DeleteTransactionThrowsExceptionIfTransactionNotFound()
    {
        var exception = Assert.Throws<Exception>(() => _repository.DeleteTransaction(1));
        Assert.That(exception?.Message, Is.EqualTo("Transaction not found."));
    }
    
    [Test]
    public async Task FilterTransactionsReturnsTransactions()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Date = DateTime.Now,
                Name = "test",
                Amount = 100,
                Tag = TransactionCategoryTag.Food,
                Type = TransactionType.Expense,
                AccountId = 1
            },
            new Transaction
            {
                Id = 2,
                Date = DateTime.Now,
                Name = "test",
                Amount = 100,
                Tag = TransactionCategoryTag.Food,
                Type = TransactionType.Expense,
                AccountId = 1
            },
            new Transaction
            {
                Id = 3,
                Date = DateTime.Now,
                Name = "test",
                Amount = 100,
                Tag = TransactionCategoryTag.Entertainment,
                Type = TransactionType.Expense,
                AccountId = 1
            }
        };
        _repository.AddTransaction(transactions[0]);
        _repository.AddTransaction(transactions[1]);
        _repository.AddTransaction(transactions[2]);
        
        var result = await _repository.FilterTransactions(TransactionType.Expense);
        
        Assert.That(result.Count(), Is.EqualTo(3));
        Assert.That(transactions, Is.EqualTo(result.ToList()));
    }
    
    [Test]
    public async Task FilterTransactionsThrowsExceptionIfNoTransactionsFound()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();

        var aggregateException = Assert.Throws<AggregateException>(() => _repository.FilterTransactions(TransactionType.Income).Wait());
        var exception = aggregateException.InnerException;
        Assert.That(exception, Is.InstanceOf<Exception>());
        Assert.That(exception.Message, Is.EqualTo($"No transaction found by {TransactionType.Income.ToString()} type."));
    }

    
    [Test]
    public async Task FinancialTransactionsReturnsTransactions()
    {
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                Id = 1,
                Date = DateTime.Now,
                Name = "test",
                Amount = 100,
                Tag = TransactionCategoryTag.Food,
                Type = TransactionType.Expense,
                AccountId = 1
            },
            new Transaction
            {
                Id = 2,
                Date = DateTime.Now,
                Name = "test",
                Amount = 100,
                Tag = TransactionCategoryTag.Food,
                Type = TransactionType.Expense,
                AccountId = 1
            },
            new Transaction
            {
                Id = 3,
                Date = DateTime.Now,
                Name = "test",
                Amount = 100,
                Tag = TransactionCategoryTag.Food,
                Type = TransactionType.Expense,
                AccountId = 1
            }
        };
        _repository.AddTransaction(transactions[0]);
        _repository.AddTransaction(transactions[1]);
        _repository.AddTransaction(transactions[2]);
        
        var result = await _repository.FinancialTransactions(TransactionCategoryTag.Food);
        
        Assert.That(transactions, Is.EqualTo(result));
    }
    
    [Test]
    public async Task FinancialTransactionsThrowsExceptionIfNoTransactionsFound()
    {
        var transaction = new Transaction
        {
            Id = 1,
            Date = DateTime.Now,
            Name = "test",
            Amount = 100,
            Tag = TransactionCategoryTag.Food,
            Type = TransactionType.Expense,
            AccountId = 1
        };
        _context.Add(transaction);
        _context.SaveChanges();
        
        var aggregateException = Assert.Throws<AggregateException>(() => _repository.FinancialTransactions(TransactionCategoryTag.Income).Wait());
        var exception = aggregateException.InnerException;
        Assert.That(exception, Is.InstanceOf<Exception>());
        Assert.That(exception.Message, Is.EqualTo($"No transaction found by {TransactionType.Income.ToString()}"));
    }
}
