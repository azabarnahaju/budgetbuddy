using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Model.Enums.TransactionEnums;
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

        var updatedTransaction = new TransactionUpdateRequest(transaction.Id, "updated test", 200,
            TransactionCategoryTag.Food, TransactionType.Income, 2);
        var result = await _repository.UpdateTransaction(updatedTransaction);

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
        
        var transactionToUpdate = new TransactionUpdateRequest(2, "updated test", 200,
            TransactionCategoryTag.Food, TransactionType.Income, 2);
        
        var aggregateException = Assert.Throws<AggregateException>(() => _repository.UpdateTransaction(transactionToUpdate).Wait());
        var exception = aggregateException?.InnerException;
        Assert.That(exception, Is.InstanceOf<Exception>());
        Assert.That(exception?.Message, Is.EqualTo("Transaction not found."));
    }
    
    
    [Test]
    public async Task DeleteTransactionRemovesTransaction()
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
        
        await _repository.DeleteTransaction(1);
    
        Assert.IsEmpty(_context.Transactions.ToList());
    }
    
    [Test]
    public async Task DeleteTransactionThrowsExceptionIfTransactionNotFound()
    {
        var exception =  Assert.ThrowsAsync<Exception>(() => _repository.DeleteTransaction(1));
        Assert.That(exception?.Message, Is.EqualTo("Transaction not found."));
    }
    
    [Test]
    public async Task FilterTransactionsReturnsTransactions()
    {
        var transactions = new List<TransactionCreateRequest>
        {
            new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1),
            new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1),
            new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1)
            
        };
        await _repository.AddTransaction(transactions[0]);
        await _repository.AddTransaction(transactions[1]);
        await _repository.AddTransaction(transactions[2]);
        
        var result = await _repository.FilterTransactions(TransactionType.Expense);
        
        Assert.That(result.Count(), Is.EqualTo(3));
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
        var transactions = new List<TransactionCreateRequest>
        {
            new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1),
            new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1),
            new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1)
        };
        await _repository.AddTransaction(transactions[0]);
        await _repository.AddTransaction(transactions[1]);
        await _repository.AddTransaction(transactions[2]);
        
        var result = await _repository.FinancialTransactions(TransactionCategoryTag.Food);
        
        Assert.That(result.Count(), Is.EqualTo(3));
    }
    
    [Test]
    public async Task FinancialTransactionsThrowsExceptionIfNoTransactionsFound()
    {
        var transactionRequest = new TransactionCreateRequest(DateTime.Now, "test", 100, TransactionCategoryTag.Food,
            TransactionType.Expense, 1);
        
        var transaction = new Transaction
        {
            Id = 1,
            Date = transactionRequest.Date,
            Name = transactionRequest.Name,
            Amount = transactionRequest.Amount,
            Tag = transactionRequest.Tag,
            Type = transactionRequest.Type,
            AccountId = transactionRequest.AccountId
        };
        
        await _context.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        var aggregateException = Assert.Throws<AggregateException>(() => _repository.FinancialTransactions(TransactionCategoryTag.Income).Wait());
        var exception = aggregateException.InnerException;
        Assert.That(exception, Is.InstanceOf<Exception>());
        Assert.That(exception.Message, Is.EqualTo($"No transaction found by {TransactionType.Income.ToString()}"));
    }
}
