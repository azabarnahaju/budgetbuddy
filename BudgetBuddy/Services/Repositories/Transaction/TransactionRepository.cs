using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;

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
    
    public async Task<IEnumerable<Transaction>> GetTransactionsByAccount(int accountId, DateTime? startDate = null, DateTime? endDate = null)
    {
        if (startDate != null && endDate != null)
        {
            return _budgetBuddyContext.Transactions.Where(t => t.AccountId == accountId && t.Date > startDate && t.Date < endDate);
        }
        return _budgetBuddyContext.Transactions.Where(t => t.AccountId == accountId);
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
        var accountExist = await _budgetBuddyContext.Transactions.AnyAsync(t => t.AccountId == accountId);
        if (!accountExist)
            throw new Exception($"No transactions found with this account ID {accountId}");
        
        return await _budgetBuddyContext.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
    }

    public async Task<Transaction> AddTransaction(TransactionCreateRequest transaction)
    {
        var transactionToAdd = new Transaction()
        {
            Name = transaction.Name,
            AccountId = transaction.AccountId,
            Amount = transaction.Amount,
            Date = transaction.Date,
            Tag = transaction.Tag,
            Type = transaction.Type
        };
        
        var newTransaction = await _budgetBuddyContext.Transactions.AddAsync(transactionToAdd);
        await _budgetBuddyContext.SaveChangesAsync();
        return newTransaction.Entity;
    }

    public async Task<Transaction> UpdateTransaction(TransactionUpdateRequest transaction)
    {
        var transactionToUpdate = await _budgetBuddyContext.Transactions.FirstOrDefaultAsync(a => a.Id == transaction.Id);
        if (transactionToUpdate is null)
        {
            throw new Exception("Transaction not found.");
        }

        _budgetBuddyContext.Transactions.Entry(transactionToUpdate).CurrentValues.SetValues(transaction);
        await _budgetBuddyContext.SaveChangesAsync();
        
        return transactionToUpdate;
    }

    public async Task DeleteTransaction(int id)
    {
        var transactionToDelete = await _budgetBuddyContext.Transactions.FirstOrDefaultAsync(r => r.Id == id);
        
        if (transactionToDelete is null)
        {
            throw new Exception("Transaction not found.");
        }
        
        _budgetBuddyContext.Transactions.Remove(transactionToDelete);
        await _budgetBuddyContext.SaveChangesAsync();
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