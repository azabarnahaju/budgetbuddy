using BudgetBuddy.Data;
using BudgetBuddy.Services.AchievementService;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.Identity.Client;

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
    private readonly IAchievementService _achievementService;
    private readonly IUserRepository _userRepository;
    private BudgetBuddyContext _dbContext;

    public AccountController(ILogger<AccountController> logger, IAccountRepository accountRepository, IAchievementService achievementService, IUserRepository userRepository)
    {
        _accountRepository = accountRepository;
        _logger = logger;
        _achievementService = achievementService;
        _userRepository = userRepository;
    }

    [HttpGet("{userId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<List<Account>>> Get(string userId)
    {
        try
        {
            var result = await _accountRepository.GetByUserId(userId);
            return Ok(new { message = "Account retrieved successfully", data = result });
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
            var user = await _userRepository.GetUserById(result.UserId);
            if (user is null) throw new Exception("User not found"); 
            await _achievementService.UpdateAccountAchievements(user);
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