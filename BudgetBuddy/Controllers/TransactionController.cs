namespace BudgetBuddy.Controllers;

using Model.Enums;
using Services.Repositories.Transaction;
using System.ComponentModel.DataAnnotations;
using Model;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<TransactionController> _logger;
    
    public TransactionController(ILogger<TransactionController> logger, ITransactionRepository transactionRepository)
    {
        _logger = logger;
        _transactionRepository = transactionRepository;
    }
    
    [HttpPost("add")]
    public ActionResult<Transaction> AddTransaction(Transaction transaction)
    {
        try
        {
            _transactionRepository.AddTransaction(transaction);
            return Ok(new { message = "Transaction added.", data = transaction });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Transaction already exists.");
            return BadRequest("Transaction already exists.");
        }
    }

    [HttpGet("transactions")]
    public ActionResult<Transaction> GetAll()
    {
        try
        {
            return Ok(new { message = "Transactions retrieved.", data = _transactionRepository.GetAllTransactions() });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting transactions");
            return BadRequest("Error getting transactions");
        }
    }
    
    [HttpGet("transactions/{id}")]
    public ActionResult<Transaction> GetTransaction(int id)
    {
        try
        {
            return Ok(new { message = "Transaction retrieved.", data = _transactionRepository.GetTransaction(id) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Transaction not found.");
            return NotFound("Transaction not found.");
        }
    }
    
    [HttpPatch("update")]
    public ActionResult<Transaction> UpdateTransaction(Transaction transaction)
    {
        try
        {
            return Ok(new { message = "Transaction updated.", data = _transactionRepository.UpdateTransaction(transaction) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating transaction");
            return NotFound("Error updating transaction");
        }
    }
    
    [HttpDelete("delete/{id}")]
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
            return NotFound("Error deleting transaction");
        }
    }

    [HttpGet("filterByType/{transactionType}")]
    public ActionResult<Transaction> FilterTransactions([Required]TransactionType transactionType)
    {
        try
        {
            return Ok(new { message = $"Transactions filtered by {transactionType}.", data = _transactionRepository.FilterTransactions(transactionType) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error filtering transactions by {transactionType}");
            return NotFound($"Error filtering transactions by {transactionType}");
        }
    }
    
    [HttpGet("filterByTag/{tag}")]
    public ActionResult<Transaction> FinancialTransactions([Required]TransactionCategoryTag tag)
    {
        try
        {
            return Ok(new { message = $"Transactions filtered by {tag}.", data = _transactionRepository.FinancialTransactions(tag) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error filtering transactions by {tag} tag.");
            return NotFound($"Error filtering transactions by {tag} tag.");
        }
    }
}