using System.Net;
using System.Net.Http.Headers;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using FluentAssertions;

namespace BudgetBuddy.IntegrationTests.IntegrationTests;

public class BasicTests : IClassFixture<BudgetBuddyWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("/account/1")]
    [InlineData("/achievement")]
    [InlineData("/achievement/1")]
    [InlineData("/goal/1")]
    [InlineData("/report")]
    [InlineData("/report/1")]
    [InlineData("/report/report/user/1")]
    [InlineData("/report/report/account/1")]
    [InlineData("/transaction/transactions")]
    [InlineData("/transaction/transactions/1")]
    [InlineData("/transaction/filterByType/testtype")]
    [InlineData("/transaction/filterByTag/testtag")]
    public async Task Get_Should_Reject_Unauthenticated_Requests(string url)
    {
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [InlineData("/account/1")]
    [InlineData("/achievement")]
    [InlineData("/achievement/1")]
    [InlineData("/goal/1")]
    [InlineData("/report/1")]
    [InlineData("/report/report/user/1")]
    [InlineData("/report/report/account/1")]
    [InlineData("/transaction/transactions")]
    [InlineData("/transaction/transactions/1")]
    [InlineData("/transaction/filterByType/Expense")]
    [InlineData("/transaction/filterByTag/Clothing")]
    public async Task Get_Should_Allow_All_Registered_Users(string url)
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("/account/1")]
    [InlineData("/achievement")]
    [InlineData("/achievement/1")]
    [InlineData("/goal/1")]
    [InlineData("/report/1")]
    [InlineData("/report")]
    [InlineData("/report/report/user/1")]
    [InlineData("/report/report/account/1")]
    [InlineData("/transaction/transactions")]
    [InlineData("/transaction/transactions/1")]
    [InlineData("/transaction/filterByType/Expense")]
    [InlineData("/transaction/filterByTag/Clothing")]
    public async Task Get_Should_Allow_All_Registered_Admins(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}