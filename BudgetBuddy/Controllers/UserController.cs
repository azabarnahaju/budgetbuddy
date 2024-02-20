using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    [HttpGet("/{userId}")]
    public ActionResult<User> Get(int userId)
    {
        try
        {
            return Ok(new { message = "User data successfully retrieved.", data = _userRepository.GetUser(userId) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "User not found.");
            return NotFound(new { message = e.Message });
        }
    }

    [HttpPatch("/update")]
    public ActionResult<User> Update(User user)
    {
        try
        {
            return Ok(new { message = "Updating user was successful.", data = _userRepository.UpdateUser(user) });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Updating user has failed.");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpDelete("/delete/{userId}")]
    public ActionResult<string> Delete(int userId)
    {
        try
        {
            _userRepository.DeleteUser(userId);
            return Ok(new { message = "User deleted successfully." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Deleting user has failed.");
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPost("/register")]
    public ActionResult<User> Register(User user)
    {
        try
        {
            return Ok(new {message = "Registration successful.", data = _userRepository.AddUser(user)});
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Registration failed.");
            return BadRequest(new { message = e.Message });
        }
    }
}