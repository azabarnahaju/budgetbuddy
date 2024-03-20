using BudgetBuddy.Services.AchievementService;
using Microsoft.AspNetCore.Authorization;

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
    private readonly IAchievementService _achievementService;

    public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository, IAchievementService achievementService)
    {
        _accountRepository = accountRepository;
        _logger = logger;
        _achievementService = achievementService;
    }

    [HttpGet("{accountId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Account>> Get(int accountId)
    {
        try
        {
            var result = await _accountRepository.GetById(accountId);
            return Ok(new { message = "Account retrieved successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Account not found");
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPost, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Account>> CreateAccount(Account account)
    {
        try
        {
            var result = await _accountRepository.CreateAccount(account);
            await _achievementService.UpdateAchievements(account.User);
            return Ok(new { message = "Account created successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Account not created");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPatch, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Account>> UpdateAccount(Account account)
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