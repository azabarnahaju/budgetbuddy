namespace BudgetBuddy.Services.Repositories.Transaction;

using Data;
using Microsoft.EntityFrameworkCore;
using Model.Enums;
using Model;

public class TransactionRepository : ITransactionRepository
{
    private readonly BudgetBuddyContext _budgetBuddyContext;

    public TransactionRepository(BudgetBuddyContext budgetBuddyContext)
    {
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
    
    public async Task<IEnumerable<Transaction>> GetTransactionByAccount(int accountId)
    {
        if (!await _budgetBuddyContext.Transactions.AnyAsync(t => t.AccountId == accountId))
            throw new Exception($"No transactions found with this account ID {accountId}");
        
        return await _budgetBuddyContext.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
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
        var transactionToUpdate = await _budgetBuddyContext.Transactions.FirstOrDefaultAsync(a => a.Id == transaction.Id);
        if (transactionToUpdate is null)
        {
            throw new Exception("Transaction not found.");
        }

        _budgetBuddyContext.Transactions.Entry(transactionToUpdate).CurrentValues.SetValues(transaction);
        await _budgetBuddyContext.SaveChangesAsync();
        
        return await _budgetBuddyContext.Transactions.FirstAsync(a => a.Id == transaction.Id) ?? throw new Exception("Transaction not found.");
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

    public async Task<IEnumerable<Transaction>> GetExpenseTransactions(int accountId, DateTime start, DateTime end)
    {
        try
        {
            var transactions = await GetTransactionByAccount(accountId);
            return transactions.Where(t => t.Date >= start && t.Date <= end);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("An error occured while retrieving transactions.");
        }
        
    }
}