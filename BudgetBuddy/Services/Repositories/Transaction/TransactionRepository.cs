using BudgetBuddy.Model.CreateModels;
using BudgetBuddy.Model.UpdateModels;

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

    public async Task<Transaction> AddTransaction(TransactionInputModel transaction)
    {
        try
        {
            var transactionToCreate = new Transaction()
            {
                Name = transaction.Name,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Date = transaction.Date,
                Tag = transaction.Tag,
                Type = transaction.Type
            };
            var newTransaction = await _budgetBuddyContext.Transactions.AddAsync(transactionToCreate);
            await _budgetBuddyContext.SaveChangesAsync();
            return newTransaction.Entity;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Cannot create new transaction.");
        }
    }

    public async Task<Transaction> UpdateTransaction(TransactionUpdateModel transaction)
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