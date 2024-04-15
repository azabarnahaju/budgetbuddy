namespace BudgetBuddy.Controllers;

using Contracts.ModelRequest;
using Contracts.ModelRequest.UpdateModels;
using Microsoft.AspNetCore.Authorization;
using Model;
using Services.Repositories.Achievement;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]"), Authorize]
public class AchievementController : ControllerBase
{
    private readonly ILogger<AchievementController> _logger;
    private readonly IAchievementRepository _achievementRepository;

    public AchievementController(ILogger<AchievementController> logger, IAchievementRepository achievementRepository)
    {
        _logger = logger;
        _achievementRepository = achievementRepository;
    }

    [HttpGet, Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<IEnumerable<Achievement>>> GetAll()
    {
        try
        {
            return Ok(new { message = "Achievements found successfully.", data = await _achievementRepository.GetAllAchievements() });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't find achievements.");
            return NotFound(new { message = "Can't find achievements." });
        }
    }
    
    [HttpGet("/Achievement/user/{userId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<IEnumerable<Achievement>>> GetAllByUserId(string userId)
    {
        try
        {
            return Ok(new
            {
                message = "Achievements found successfully.",
                data = await _achievementRepository.GetAllAchievementsByUserId(userId)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't find achievements.");
            return NotFound(new { message = "Can't find achievements." });
        }
    }

    [HttpGet("/Achievement/{achievementId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<Achievement>> Get(int achievementId)
    {
        try
        {
            return Ok(new
            {
                message = "Achievement found successfully.", data = await _achievementRepository.GetAchievement(achievementId)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Achievement couldn't be found.");
            return NotFound(new { message = e.Message });
        }    
    }
    
    // admin functionality
    [HttpPost("/Achievement/add"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Achievement>> Add(IEnumerable<AchievementCreateRequest> achievements)
    {
        try
        {
            return Ok(new
            {
                message = "Achievement(s) successfully added.",
                data = await _achievementRepository.AddAchievement(achievements)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Achievement can't be found");
            return NotFound(new { message = e.Message });
        }
    }
    
    // admin functionality
    [HttpDelete("delete/{achievementId}"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult<string>> Delete(int achievementId)
    {
        try
        {
            await _achievementRepository.DeleteAchievement(achievementId);
            return Ok(new { message = "Deleting achievement was successful." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Deleting achievement has failed.");
            return BadRequest(new { message = e.Message });
        }
    }

    // admin functionality
    [HttpPatch("update"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<Achievement>> Update(AchievementUpdateRequest achievement)
    {
        try
        {
            return Ok(new
            {
                message = "Updating message was successful.",
                data = await _achievementRepository.UpdateAchievement(achievement)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Updating achievement has failed.");
            return BadRequest(new { message = e.Message });
        }
    }
}