using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Goal;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("[controller]")]
public class GoalController : ControllerBase
{
    private readonly IGoalRepository _goalRepository;
    private readonly ILogger<AccountController> _logger;
    
    public GoalController(ILogger<AccountController> logger, IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetGoalsByAccountId(int userId)
    {
        try
        {
            var result = await _goalRepository.GetAllGoalsByAccountId(userId);
            return Ok(new { message = "Goals retrieved successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Goals not retrieved");
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateGoal(GoalModel goalModel)
    {
        try
        {
            var result = await _goalRepository.CreateGoal(goalModel);
            return Ok(new { message = "Goal created successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Goal not created");
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPatch]
    public async Task<IActionResult> UpdateGoal(GoalModel goalModel)
    {
        try
        {
            var result = await _goalRepository.UpdateGoal(goalModel);
            return Ok(new { message = "Goal updated successfully", data = result });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Goal not updated");
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpDelete("{goalId}")]
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