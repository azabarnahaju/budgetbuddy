using Microsoft.AspNetCore.Authorization;

namespace BudgetBuddy.IntegrationTests.JwtAuthenticationTest;

public class FakeAuthorizationPolicy : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        return Task.FromResult(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());
    }
}