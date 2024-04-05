using System.Net.Http.Headers;
using System.Text;
using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using Newtonsoft.Json;

namespace BudgetBuddy.IntegrationTests.IntegrationTests;

public class PostEndpointTests : IClassFixture<BudgetBuddyWebApplicationFactory<Program>>
{
    private readonly BudgetBuddyWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public PostEndpointTests(BudgetBuddyWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }
    
    [Theory]
    [InlineData("/account")]
    public async Task Post_Account_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var expectedResult = new Account(){Id = 2, Type = "Card", Balance = 1500, Name = "Revolut", UserId = "1"};
        var accountToCreate = new AccountCreateRequest(1500, "Revolut", "Card", "1");
        
        var content = new StringContent(JsonConvert.SerializeObject(accountToCreate), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, content);
        
        response.EnsureSuccessStatusCode();
        Account actualResult = await ConvertResponseData<Account>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    
    [Theory]
    [InlineData("/achievement/add")]
    public async Task Post_Achievement_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var expectedResult = new List<Achievement>()
            { new () { Id = 2, Name = "Test", Description = "Test" } };
        var achievementToCreate = new List<AchievementCreateRequest> {new ("Test", "Test")};
        
        var content = new StringContent(JsonConvert.SerializeObject(achievementToCreate), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, content);
        
        response.EnsureSuccessStatusCode();
        List<Achievement> actualResult = await ConvertResponseData<List<Achievement>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    
    [Theory]
    [InlineData("/goal")]
    public async Task Post_Goal_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var expectedResult = new Goal()
        {
            Id = 2, Type = GoalType.Income, UserId = "1", Completed = false,
            CurrentProgress = 10, AccountId = 1, Target = 150
        };
        var goalToCreate = new GoalCreateRequest("1", 1, GoalType.Income, 150, 10, false);
        
        var content = new StringContent(JsonConvert.SerializeObject(goalToCreate), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, content);
        
        response.EnsureSuccessStatusCode();
        Goal actualResult = await ConvertResponseData<Goal>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    
    [Theory]
    [InlineData("/transaction/add")]
    public async Task Post_Transaction_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    
        var expectedResult = new Transaction()
        {
            Id = 0, Type = TransactionType.Expense, Name = "Test", AccountId = 1, Date = new DateTime(2022, 02, 02),
            Amount = 150, Tag = TransactionCategoryTag.Clothing
        };
        var transactionToCreate = new TransactionCreateRequest(new DateTime(2022, 02, 02), "Test", 0, TransactionCategoryTag.Clothing, TransactionType.Expense, 1);
        
        var content = new StringContent(JsonConvert.SerializeObject(transactionToCreate), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(url, content);
        
        response.EnsureSuccessStatusCode();
        Transaction actualResult = await ConvertResponseData<Transaction>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    async Task<dynamic> ConvertResponseData<T>(HttpResponseMessage response)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        var responseData = JsonConvert.DeserializeObject<dynamic>(responseContent);
        var result = responseData.data;
        T converted = result.ToObject<T>();
        return converted;
    }
}
