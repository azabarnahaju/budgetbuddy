using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Achievement;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("[controller]")]
public class AchievementController : ControllerBase
{
    private readonly ILogger<AchievementController> _logger;
    private readonly IAchievementRepository _achievementRepository;

    public AchievementController(ILogger<AchievementController> logger, IAchievementRepository achievementRepository)
    {
        _logger = logger;
        _achievementRepository = achievementRepository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Achievement>> GetAll()
    {
        try
        {
            return Ok(new { message = "Achievements found successfully.", data = _achievementRepository.GetAllAchievements() });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Can't find achievements.");
            return NotFound(new { message = "Can't find achievements." });
        }
    }

    [HttpGet("/Achievement/{achievementId}")]
    public ActionResult<Achievement> Get(int achievementId)
    {
        try
        {
            return Ok(new
            {
                message = "Achievement found successfully.", data = _achievementRepository.GetAchievement(achievementId)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Achievement couldn't be found.");
            return NotFound(new { message = e.Message });
        }    
    }
    
    // admin functionality
    [HttpPost("/Achievement/add")]
    public ActionResult<Achievement> Add(IEnumerable<Achievement> achievements)
    {
        try
        {
            return Ok(new
            {
                message = "Achievement(s) successfully added.",
                data = _achievementRepository.AddAchievement(achievements)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Achievement can't be found");
            return NotFound(new { message = e.Message });
        }
    }
    
    // admin functionality
    [HttpDelete("delete/{achievementId}")]
    public ActionResult<string> Delete(int achievementId)
    {
        try
        {
            _achievementRepository.DeleteAchievement(achievementId);
            return Ok(new { message = "Deleting achievement was successful." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Deleting achievement has failed.");
            return BadRequest(new { message = e.Message });
        }
    }

    // admin functionality
    [HttpPatch("update")]
    public ActionResult<Achievement> Update(Achievement achievement)
    {
        try
        {
            return Ok(new
            {
                message = "Updating message was successful.",
                data = _achievementRepository.UpdateAchievement(achievement)
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Updating achievement has failed.");
            return BadRequest(new { message = e.Message });
        }
    }
}