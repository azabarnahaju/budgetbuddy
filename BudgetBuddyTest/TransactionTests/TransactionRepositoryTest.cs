namespace BudgetBuddyTest.TransactionTests;

using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Repositories.Transaction;
using Microsoft.Extensions.Logging;
using Moq;

[TestFixture]
public class TransactionRepositoryTest
{
    private ITransactionRepository _transactionRepository;
    private ILogger<TransactionRepository> _logger;
    

    [SetUp]
    public void SetUp()
    {
        _logger = new Mock<ILogger<TransactionRepository>>().Object;
        _transactionRepository = new TransactionRepository(_logger);
    }
    
    [Test]
    public void AddTransactionReturnsTransaction()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
         _transactionRepository.AddTransaction(transaction);
        
        Assert.That(transaction, Is.EqualTo(_transactionRepository._transactions.First()));
    }
    
    [Test]
    public void AddTransactionThrowsExceptionIfTransactionExists()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository.AddTransaction(transaction);
        
        var exception = Assert.Throws<Exception>(() => _transactionRepository.AddTransaction(transaction));
        Assert.That(exception?.Message, Is.EqualTo("Transaction already exists."));
    }
    
    [Test]
    public void GetAllTransactionsReturnsTransactions()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction);
        
        var result = _transactionRepository.GetAllTransactions().ToList();
        
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(_transactionRepository._transactions, Is.EqualTo(result.ToList()));
    }
    
    [Test]
    public void GetTransactionReturnsTransaction()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction);
        
        var result = _transactionRepository.GetTransaction(1);
        
        Assert.That(transaction, Is.EqualTo(result));
    }
    
    [Test]
    public void GetTransactionThrowsExceptionIfTransactionNotFound()
    {
        var exception = Assert.Throws<Exception>(() => _transactionRepository.GetTransaction(1));
        Assert.That(exception?.Message, Is.EqualTo("Transaction not found"));
    }
    
    [Test]
    public void UpdateTransactionReturnsTransaction()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction);
        
        var result = _transactionRepository.UpdateTransaction(transaction);
        
        Assert.That(transaction, Is.EqualTo(result));
    }
    
    [Test]
    public void UpdateTransactionThrowsExceptionIfTransactionNotFound()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        
        var exception = Assert.Throws<Exception>(() => _transactionRepository.UpdateTransaction(transaction));
        Assert.That(exception?.Message, Is.EqualTo("Transaction not found."));
    }
    
    [Test]
    public void DeleteTransactionRemovesTransaction()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction);
        
        _transactionRepository.DeleteTransaction(1);
        
        Assert.IsEmpty(_transactionRepository._transactions);
    }
    
    [Test]
    public void DeleteTransactionThrowsExceptionIfTransactionNotFound()
    {
        var exception = Assert.Throws<Exception>(() => _transactionRepository.DeleteTransaction(1));
        Assert.That(exception?.Message, Is.EqualTo("Transaction is not found by ID."));
    }
    
    [Test]
    public void FilterTransactionsReturnsTransactions()
    {
        var transaction1 = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction1);
        var transaction2 = new Transaction(2, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction2);
        var transaction3 = new Transaction(2, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Income, 1);
        _transactionRepository._transactions.Add(transaction3);
        
        var result = _transactionRepository.FilterTransactions(TransactionType.Expense);
        
        Assert.That(result.ToList().Count(), Is.EqualTo(2));
        Assert.That(new List<Transaction> { transaction1, transaction2 }, Is.EqualTo(result.ToList()));
    }
    
    [Test]
    public void FilterTransactionsThrowsExceptionIfNoTransactionsFound()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction);
        
        var exception = Assert.Throws<Exception>(() => _transactionRepository.FilterTransactions(TransactionType.Income));
        Assert.That(exception?.Message, Is.EqualTo("No transaction found by type."));
    }
    
    [Test]
    public void FinancialTransactionsReturnsTransactions()
    {
        var transaction1 = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction1);
        var transaction2 = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction2);
        var transaction3 = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Entertainment, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction3);
        
        var result = _transactionRepository.FinancialTransactions(TransactionCategoryTag.Food);
        
        Assert.That(new List<Transaction> { transaction1, transaction2 }, Is.EqualTo(result.ToList()));
    }
    
    [Test]
    public void FinancialTransactionsThrowsExceptionIfNoTransactionsFound()
    {
        var transaction = new Transaction(1, DateTime.Now, "test", 100, TransactionCategoryTag.Food, TransactionType.Expense, 1);
        _transactionRepository._transactions.Add(transaction);
        
        var exception = Assert.Throws<Exception>(() => _transactionRepository.FinancialTransactions(TransactionCategoryTag.Entertainment));
        Assert.That(exception?.Message, Is.EqualTo("No transaction found by Entertainment"));
    }
}
