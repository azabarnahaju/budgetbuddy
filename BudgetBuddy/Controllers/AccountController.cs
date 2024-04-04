using BudgetBuddy.Contracts.ResponseModels;

namespace BudgetBuddy.Controllers;

using Model;
using Services.Repositories.Account;
using Microsoft.AspNetCore.Mvc;
using Contracts.ModelRequest;
using Contracts.ModelRequest.UpdateModels;
using Microsoft.AspNetCore.Authorization;

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

    [HttpGet("{userId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<AccountResponse>> Get(string userId)
    {
        try
        {
            var result = await _accountRepository.GetByUserId(userId);
            return Ok(new AccountResponse("Account retrieved successfully", result));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Account not found");
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Account>> CreateAccount(AccountCreateRequest account)
    {
        try
        {
            var result = await _accountRepository.CreateAccount(account);
            return Ok(new { message = "Account created successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Account not created");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPatch, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Account>> UpdateAccount(AccountUpdateRequest account)
    {
        try
        {
            var result = await _accountRepository.UpdateAccount(account);
            return Ok(new { message = "Account updated successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot update account.");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("{accountId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<bool>> DeleteAccount(int accountId)
    {
        try
        {
            await _accountRepository.DeleteAccount(accountId);
            return Ok(new { message = "Account deleted successfully", data = true });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot delete account.");
            return BadRequest(new { message = e.Message });
        }
    }
}