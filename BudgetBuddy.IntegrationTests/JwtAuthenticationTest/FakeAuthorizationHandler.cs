using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BudgetBuddy.IntegrationTests.JwtAuthenticationTest;

public class FakeAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public FakeAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        
        if (httpContext.User.Identity.IsAuthenticated && httpContext.Request.Headers.ContainsKey("Authorization"))
        {
            context.Succeed(requirement);
        }
        
        else if (httpContext.User.Identity.IsAuthenticated && httpContext.Request.Cookies.ContainsKey("Cookie_Name"))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}