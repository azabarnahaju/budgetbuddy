namespace BudgetBuddy.Services.Repositories.Transaction;

using Data;
using Microsoft.EntityFrameworkCore;
using Model.Enums;
using Model;

public class TransactionRepository : ITransactionRepository
{
    private readonly ILogger<TransactionRepository> _logger;
    private readonly BudgetBuddyContext _budgetBuddyContext;

    public TransactionRepository(ILogger<TransactionRepository> logger, BudgetBuddyContext budgetBuddyContext)
    {
        _logger = logger;
        _budgetBuddyContext = budgetBuddyContext;
    }

    public async Task<IEnumerable<Transaction>> GetAllTransactions()
    {
        return await _budgetBuddyContext.Transactions.ToListAsync();
    }
    
    public async Task<Transaction> GetTransaction(int id)
    {
        var transaction = await _budgetBuddyContext.Transactions.FirstOrDefaultAsync(r => r.Id == id);

        if (transaction is null)
        {
            throw new Exception("Transaction not found.");
        }

        return transaction;
    }

    public void AddTransaction(Transaction transaction)
    {
        var transactionToAdd = _budgetBuddyContext.Transactions.FirstOrDefault(r => r.Id == transaction.Id);
        
        if (transactionToAdd != null)
        {
            throw new Exception("Transaction already exists.");
        }
        
        _budgetBuddyContext.Transactions.Add(transaction);
        _budgetBuddyContext.SaveChanges();
    }

    public async Task<Transaction> UpdateTransaction(Transaction transaction)
    {
        var transactionToUpdate = await _budgetBuddyContext.Transactions.FirstOrDefaultAsync(r => r.Id == transaction.Id);

        if (transactionToUpdate is null)
        {
            throw new Exception("Transaction not found.");
        }
        
        _budgetBuddyContext.Transactions.Update(transaction);
        await _budgetBuddyContext.SaveChangesAsync();
        return transaction;
    }

    public void DeleteTransaction(int id)
    {
        var transactionToDelete = _budgetBuddyContext.Transactions.FirstOrDefault(r => r.Id == id);
        
        if (transactionToDelete is null)
        {
            throw new Exception("Transaction not found.");
        }
        
        _budgetBuddyContext.Transactions.Remove(transactionToDelete);
        _budgetBuddyContext.SaveChanges();
    }

    public async Task<IEnumerable<Transaction>> FilterTransactions(TransactionType transactionType)
    {
        var filteredTransactions = await _budgetBuddyContext.Transactions.Where(t => t.Type == transactionType).ToListAsync();

        if (!filteredTransactions.Any())
        {
            throw new Exception($"No transaction found by {transactionType.ToString()} type.");
        }

        return filteredTransactions;
    }

    public async Task<IEnumerable<Transaction>> FinancialTransactions(TransactionCategoryTag tag)
    {
        var filteredTransactions = await _budgetBuddyContext.Transactions.Where(t => t.Tag == tag).ToListAsync();

        if (!filteredTransactions.Any())
        {
            throw new Exception($"No transaction found by {tag.ToString()}");
        }

        return filteredTransactions;
    }
}