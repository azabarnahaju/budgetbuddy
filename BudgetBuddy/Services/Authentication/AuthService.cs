using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Services.Authentication;

using Model;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    
    public AuthenticationService
//     private readonly IUserRepository _userRepository;
//     private readonly ILogger<AuthenticationService> _logger;
//     
//     public AuthenticationService(ILogger<AuthenticationService> logger, IUserRepository userRepository)
//     {
//         _logger = logger;
//         _userRepository = userRepository;
//     }
//     
//     public async Task<bool> Authenticate(AuthParams authParams)
//     {
//         try
//         {
//             var user = await _userRepository.GetUser(authParams.Email);
//             return user.Password == authParams.Password;
//         }
//         catch (InvalidDataException e)
//         {
//             _logger.LogError(e, "User not found.");
//             throw new Exception("User not found.");
//         }
//         catch (Exception e)
//         {
//             _logger.LogError(e, "Error while trying to log in.");
//             throw new Exception("Error while trying to log you in.");
//         }
//     }
}