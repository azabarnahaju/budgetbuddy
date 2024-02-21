namespace BudgetBuddy.Controllers;

using Model;
using Services.Repositories.Account;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
        _logger = logger;
    }

    [HttpGet("{accountId}")]
    public ActionResult<Account> Get(int accountId)
    {
        try
        {
            var result = _accountRepository.GetById(accountId);
            return Ok(new { message = "Account retrieved successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Account not found");
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost]
    public ActionResult<Account> CreateAccount(Account account)
    {
        try
        {
            var result = _accountRepository.CreateAccount(account);
            return Ok(new { message = "Account created successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Account not created");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPatch]
    public ActionResult<Account> UpdateAccount(Account account)
    {
        try
        {
            var result = _accountRepository.UpdateAccount(account);
            return Ok(new { message = "Account updated successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot update account.");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("{accountId}")]
    public ActionResult<bool> DeleteAccount(int accountId)
    {
        try
        {
            _accountRepository.DeleteAccount(accountId);
            return Ok(new { message = "Account deleted successfully", data = true });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot delete account.");
            return BadRequest(new { message = e.Message });
        }
    }
}