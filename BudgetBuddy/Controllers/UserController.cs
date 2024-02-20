using System.Security.Claims;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using IAuthenticationService = BudgetBuddy.Services.Authentication.IAuthenticationService;

namespace BudgetBuddy.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;
    private readonly IAuthenticationService _authenticationService;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _authenticationService = authenticationService;
    }

    [HttpGet("/User/{userId}")]
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

    [HttpPatch("/User/update")]
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

    [HttpDelete("{userId}")]
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

    [HttpPost("/Register")]
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

    [HttpPost("/Login")]
    public async Task<ActionResult<User>> Login(AuthParams authParams)
    {
        try
        {
            var success = _authenticationService.Authenticate(authParams);
            if (!success) throw new Exception("Invalid login credentials.");
    
            var user = _userRepository.GetUser(authParams.Email);
            
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("Email", user.Email)
            };
            
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(15),
                IsPersistent = true
            };
    
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
            
            _logger.LogInformation($"{user.Username} logged in at {DateTime.Now}");
    
            return Ok(new { message = "Successfully logged in." });
    
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while trying to log in.");
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpGet("AmISignedIn")]
    public IActionResult SignedIn()
    {
        bool isAuthenticated = HttpContext.User.Identity.IsAuthenticated;
        if (isAuthenticated)
        {
            string userName = HttpContext.User.Identity.Name;
            return Ok(new { isAuthenticated, userName });
        }
        return Unauthorized(new {error = "Not logged in"});
    }
    
    [HttpPost("Logout")]
    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new {message = "Successfully logged out."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new {error = "Error while signing out." });
        }
    }
}