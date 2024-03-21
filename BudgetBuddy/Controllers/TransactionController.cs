using BudgetBuddy.Data;
using BudgetBuddy.Services.AchievementService;
using BudgetBuddy.Services.GoalServices;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Controllers;

using Services;
using Contracts.ModelRequest.CreateModels;
using Contracts.ModelRequest.UpdateModels;
using Microsoft.AspNetCore.Authorization;
using Model.Enums;
using Services.Repositories.Transaction;
using System.ComponentModel.DataAnnotations;
using Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]"), Authorize]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<TransactionController> _logger;
    private readonly IGoalService _goalService;
    private readonly IAchievementService _achievementService;
    private readonly BudgetBuddyContext _dbContext;
    
    public TransactionController(ILogger<TransactionController> logger, ITransactionRepository transactionRepository, IGoalService goalService, IAchievementService achievementService, BudgetBuddyContext dbContext)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
        _goalService = goalService;
        _achievementService = achievementService;
        _dbContext = dbContext;
    }
    
    [HttpPost("add"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Transaction>> AddTransaction(TransactionCreateRequest transaction)
    {
        try
        {
            var result = await _transactionRepository.AddTransaction(transaction);
            await _goalService.UpdateGoalProcess(result);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Accounts.FirstOrDefault(a => a.Id == result.AccountId) != null);
            await _achievementService.UpdateAchievements(user);
            return Ok(new { message = "Transaction added.", data = transaction });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Transaction already exists.");
            return BadRequest(new { message = "Transaction already exists." });
        }
    }

    [HttpGet("transactions"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Transaction>> GetAll()
    {
        try
        {
            var transactions = await _transactionRepository.GetAllTransactions();
            return Ok(new { message = "Transactions retrieved.", data = transactions });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting transactions");
            return BadRequest(new { message = "Error getting transactions" });
        }
    }
    
    [HttpGet("transactions/{id}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        try
        {
            var transaction = await _transactionRepository.GetTransaction(id);
            return Ok(new { message = "Transaction retrieved.", data = transaction });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Transaction not found.");
            return NotFound(new { message = "Transaction not found." });
        }
    }
    
    [HttpPatch("update"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Transaction>> UpdateTransaction(TransactionUpdateRequest transaction)
    {
        try
        {
            var transactionToUpdate = await _transactionRepository.UpdateTransaction(transaction); 
            return Ok(new { message = "Transaction updated.", data = transactionToUpdate });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating transaction");
            return NotFound(new { message = "Error updating transaction" });
        }
    }
    
    [HttpDelete("delete/{id}"), Authorize(Roles = "Admin, User")]
    public ActionResult<Transaction> DeleteTransaction(int id)
    {
        try
        {
            _transactionRepository.DeleteTransaction(id);
            return Ok(new { message = "Transaction deleted." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting transaction");
            return NotFound(new { message = "Error deleting transaction" });
        }
    }

    [HttpGet("filterByType/{transactionType}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Transaction>> FilterTransactions([Required]TransactionType transactionType)
    {
        try
        {
            var filteredTransactions = await _transactionRepository.FilterTransactions(transactionType);
            return Ok(new { message = $"Transactions filtered by {transactionType}.", data = filteredTransactions });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error filtering transactions by {transactionType}");
            return NotFound(new { message = $"Error filtering transactions by {transactionType}" });
        }
    }
    
    [HttpGet("filterByTag/{tag}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Transaction>> FinancialTransactions([Required]TransactionCategoryTag tag)
    {
        try
        {
            var filteredTransactions = await _transactionRepository.FinancialTransactions(tag);
            return Ok(new { message = $"Transactions filtered by {tag}.", data = filteredTransactions });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error filtering transactions by {tag} tag.");
            return NotFound(new { message = $"Error filtering transactions by {tag} tag." });
        }
    }
}