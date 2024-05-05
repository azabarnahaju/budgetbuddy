using System.Net.Http.Headers;
using System.Text;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;
using Newtonsoft.Json;

namespace BudgetBuddy.IntegrationTests.IntegrationTests;

public class UpdateEndpointTests : IClassFixture<BudgetBuddyWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("/account")]
    public async Task Update_Account_Return_Success(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var expectedResult = new Account() { Balance = 25845, Id = 1, Name = "NewName", Type = "PayPal", UserId = "1" };
        var accountToUpdate = new AccountUpdateRequest(expectedResult.Id, expectedResult.Balance, expectedResult.Name, expectedResult.Type, expectedResult.UserId);
        var content = new StringContent(JsonConvert.SerializeObject(accountToUpdate), Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(url, content);

        Account currentResult = await ConvertResponseData<Account>(response);
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(expectedResult, currentResult);
    }
    
    
    [Theory]
    [InlineData("/achievement/update")]
    public async Task Update_Achievement_Return_Success(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var expectedResult = new Achievement("UpdatedName", AchievementType.Exploration, 30, AchievementObjectiveType.Account) { Id = 1, Description = "UpdatedDescription"};
        var achievementToUpdate = new AchievementUpdateRequest(expectedResult.Id, expectedResult.Name, expectedResult.Type, expectedResult.Criteria, expectedResult.Objective, expectedResult.Description);
        var content = new StringContent(JsonConvert.SerializeObject(achievementToUpdate), Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(url, content);

        Achievement currentResult = await ConvertResponseData<Achievement>(response);
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(expectedResult, currentResult);
    }
    
    
    [Theory]
    [InlineData("/transaction/update")]
    public async Task Update_Transaction_Returns_Success(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var expectedResult = new Transaction() { Id = 1, Name = "NewName", Type = TransactionType.Expense, AccountId = 10, Amount = 17000, Tag = TransactionCategoryTag.Food, Date = new DateTime(2022, 02, 02)};
        var achievementToUpdate = new TransactionUpdateRequest(expectedResult.Id, expectedResult.Name, expectedResult.Amount, expectedResult.Tag, expectedResult.Type, expectedResult.AccountId);
        var content = new StringContent(JsonConvert.SerializeObject(achievementToUpdate), Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(url, content);

        Transaction currentResult = await ConvertResponseData<Transaction>(response);
        
        response.EnsureSuccessStatusCode();
        Assert.Equal(expectedResult, currentResult);
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

