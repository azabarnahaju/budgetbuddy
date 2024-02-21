using System.Net.Security;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.User;

namespace BudgetBuddy.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<AuthenticationService> _logger;
    
    public AuthenticationService(ILogger<AuthenticationService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }
    
    public bool Authenticate(AuthParams authParams)
    {
        try
        {
            var user = _userRepository.GetUser(authParams.Email);
            return user.Password == authParams.Password;
        }
        catch (InvalidDataException e)
        {
            _logger.LogError(e, "User not found.");
            throw new Exception("User not found.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while trying to log in.");
            throw new Exception("Error while trying to log you in.");
        }
    }
}