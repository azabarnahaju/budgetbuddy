namespace BudgetBuddy.Services.Repositories.Transaction;

using Model.Enums;
using Model;

public class TransactionRepository : ITransactionRepository
{
    private IList<Transaction> _transactions = new List<Transaction>();
    private ILogger<TransactionRepository> _logger;

    public TransactionRepository(ILogger<TransactionRepository> logger)
    {
        _logger = logger;
    }

    public IEnumerable<Transaction> GetAllTransactions()
    {
        _logger.LogInformation("Getting all transactions.");
        return _transactions;
    }
    
    public Transaction GetTransaction(int id)
    {
        
        return _transactions.All(transaction => transaction.Id != id) ? throw new Exception("Transaction not found") : _transactions.First(transaction => transaction.Id == id);
    }

    public void AddTransaction(Transaction transaction)
    {
        if (_transactions.Any(r => r.Id == transaction.Id)) throw new Exception("Transaction already exists.");

        _transactions.Add(transaction);
        _logger.LogInformation("Transaction added.");
    }

    public Transaction UpdateTransaction(Transaction transaction)
    {
        if(_transactions.FirstOrDefault(r => r.Id == transaction.Id) is null ) throw new Exception("Transaction not found.");
        
        _transactions = _transactions.Select(r => transaction.Id == r.Id ? transaction : r).ToList();
        return _transactions.First(r => r.Id == transaction.Id);
    }

    public void DeleteTransaction(int id)
    {
        if (_transactions.All(r => r.Id != id)) throw new Exception("Transaction is not found by ID.");

        _transactions = _transactions.Where(r => r.Id != id).ToList();
    }

    public IEnumerable<Transaction> FilterTransactions(TransactionType transactionType)
    {
        if(_transactions.All(r => r.Type != transactionType)) throw new Exception("No transaction found by type.");
        
        return _transactions.Where(r => r.Type == transactionType);
    }

    public IEnumerable<Transaction> FinancialTransactions(TransactionCategoryTag tag)
    {
        if(_transactions.All(r => r.Tag != tag)) throw new Exception($"No transaction found by {tag.ToString()}");
        
        return _transactions.Where(r => r.Tag == tag);
    }
}