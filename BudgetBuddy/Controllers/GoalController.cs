using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Data;
using BudgetBuddy.Services.AchievementService;

namespace BudgetBuddy.Controllers;

using Microsoft.AspNetCore.Authorization;
using Services.Repositories.Goal;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]"), Authorize]
public class GoalController : ControllerBase
{
    private readonly IGoalRepository _goalRepository;
    private readonly ILogger<AccountController> _logger;
    private readonly IAchievementService _achievementService;
    private readonly BudgetBuddyContext _dbContext;
    
    public GoalController(ILogger<AccountController> logger, IGoalRepository goalRepository, IAchievementService achievementService, BudgetBuddyContext dbContext)
    {
        _goalRepository = goalRepository;
        _logger = logger;
        _achievementService = achievementService;
        _dbContext = dbContext;
    }

    [HttpGet("{accountId}"), Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetGoalsByAccountId(int accountId)
    {
        try
        {
            var result = await _goalRepository.GetAllGoalsByAccountId(accountId);
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == result[0].UserId);
            await _achievementService.UpdateGoalAchievements(user);
            return Ok(new { message = "Goals retrieved successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Goals not retrieved");
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPost, Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> CreateGoal(GoalCreateRequest goal)
    {
        try
        {
            var result = await _goalRepository.CreateGoal(goal);
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == result.UserId);
            await _achievementService.UpdateGoalAchievements(user);
            return Ok(new { message = "Goal created successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Goal not created");
            return BadRequest(new { message = e.Message });
        }
    }
    
    
    [HttpDelete("{goalId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<bool>> DeleteGoal(int goalId)
    {
        try
        {
            await _goalRepository.DeleteGoal(goalId);
            return Ok(new { message = "Goal deleted successfully", data = true });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cannot delete goal.");
            return BadRequest(new { message = e.Message });
        }
    }
}