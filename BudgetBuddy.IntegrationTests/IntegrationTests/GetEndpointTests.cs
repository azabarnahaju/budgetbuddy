using System.Net;
using System.Net.Http.Headers;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using FluentAssertions;
using Newtonsoft.Json;

namespace BudgetBuddy.IntegrationTests.IntegrationTests;

public class GetEndpointTests : IClassFixture<BudgetBuddyWebApplicationFactory<Program>>
{
    [Theory]
    [InlineData("/account/1")]
    public async Task Get_Accounts_By_UserId_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Account>()
        {
            new()
            {
                Id = 1, UserId = "1", Date = new DateTime(2022, 02, 02), Balance = 1500, Name = "Test", Type = "Test"
            },
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Account> actualResult = await ConvertResponseData<List<Account>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/account/3")]
    public async Task Get_Accounts_By_UserId_Return_Empty_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Account>();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Account> actualResult = await ConvertResponseData<List<Account>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/achievement")]
    public async Task Get_AllAchievements_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Achievement>
        {
            new Achievement
                { Id = 1, Name = "Pioneer", Description = "You've recorded your first expense transaction!" },
            new Achievement
                { Id = 2, Name = "Big Spender", Description = "You've recorded 5 expense transactions!" },
            new Achievement
                { Id = 3, Name = "Money Bags", Description = "You've recorded 10 expense transactions!" },

            new Achievement
                { Id = 4, Name = "Money Maker", Description = "You've recorded your first income transaction!" },
            new Achievement
                { Id = 5, Name = "Wealth Builder", Description = "You've recorded 5 income transactions!" },
            new Achievement
                { Id = 6, Name = "Financial Wizard", Description = "You've recorded 10 income transactions!" },

            new Achievement { Id = 7, Name = "Account Holder", Description = "You've created your first account!" },

            new Achievement
                { Id = 8, Name = "Penny Pincher", Description = "You've saved up $500 in your account!" },
            new Achievement { Id = 9, Name = "Frugal", Description = "You've saved up $1000 in your account!" },
            new Achievement { Id = 10, Name = "Thrifty", Description = "You've saved up $1500 in your account!" },

            new Achievement { Id = 11, Name = "Goal Setter", Description = "You've set your first goal!" },
            new Achievement { Id = 12, Name = "Goal Achiever", Description = "You've set 3 goals!" },
            new Achievement { Id = 13, Name = "Master of Goals", Description = "You've set 5 goals!" },

            new Achievement { Id = 14, Name = "Goal Getter", Description = "You've completed your first goal!" },
            new Achievement { Id = 15, Name = "Goal Digger", Description = "You've completed 3 goals!" },
            new Achievement { Id = 16, Name = "Goal Crusher", Description = "You've completed 5 goals!" },

            new Achievement
                { Id = 17, Name = "Five-Star Dabbler", Description = "You've used 5 different categories!" },
            new Achievement { Id = 18, Name = "Jack of All Trades", Description = "You've used 10 categories!" },
            new Achievement { Id = 19, Name = "Master of All", Description = "You've used ALL categories!" }
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Achievement> actualResult = await ConvertResponseData<List<Achievement>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/achievement/1")]
    public async Task Get_AchievementById_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new Achievement
            { Id = 1, Name = "Pioneer", Description = "You've recorded your first expense transaction!" };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        Achievement actualResult = await ConvertResponseData<Achievement>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/achievement/50")]
    public async Task Get_AchievementById_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/goal/1")]
    public async Task Get_GoalByAccountId_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Goal>()
        {
            new ()
            {
                AccountId = 1, UserId = "1", Id = 1, Completed = false, CurrentProgress = 0,
                StartDate = new DateTime(2022, 02, 02),
                Type = GoalType.Income, Target = 100
            }
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Goal> actualResult = await ConvertResponseData<List<Goal>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/goal/20")]
    public async Task Get_GoalByAccountId_Returns_Empty_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var expected = Array.Empty<Goal>();
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var actualResult = await ConvertResponseData<List<Goal>>(response);
        Assert.Equivalent(expected, actualResult);
    }
    
    [Theory]
    [InlineData("/report/1")]
    public async Task Get_ReportById_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new Report()
        {
            Id = 1, AccountId = 1, ReportType = ReportType.Monthly, StartDate = new DateTime(2022, 02, 02),
            EndDate = new DateTime(2022, 02, 02), SumExpense = 100, AverageSpendingDaily = 100,
            AverageSpendingTransaction = 100, MostSpendingTag = TransactionCategoryTag.Education,
            MostSpendingDay = new DateTime(2022, 02, 02), BiggestExpense = 100, CreatedAt = new DateTime(2022, 02, 02),
            SumIncome = 100, Categories = new HashSet<TransactionCategoryTag>(),
            SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        Report actualResult = await ConvertResponseData<Report>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/report/3")]
    public async Task Get_ReportById_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/report")]
    public async Task Get_AllReports_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Report>
        {
            new()
            {
                Id = 1, AccountId = 1, ReportType = ReportType.Monthly, StartDate = new DateTime(2022, 02, 02),
                EndDate = new DateTime(2022, 02, 02), SumExpense = 100, AverageSpendingDaily = 100,
                AverageSpendingTransaction = 100, MostSpendingTag = TransactionCategoryTag.Education,
                MostSpendingDay = new DateTime(2022, 02, 02), BiggestExpense = 100,
                CreatedAt = new DateTime(2022, 02, 02),
                SumIncome = 100, Categories = new HashSet<TransactionCategoryTag>(),
                SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()
            }
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Report> actualResult = await ConvertResponseData<List<Report>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/report/report/user/1")]
    public async Task Get_ReportsByUserId_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new Report
        {

            Id = 1, AccountId = 1, ReportType = ReportType.Monthly, StartDate = new DateTime(2022, 02, 02),
            EndDate = new DateTime(2022, 02, 02), SumExpense = 100, AverageSpendingDaily = 100,
            AverageSpendingTransaction = 100, MostSpendingTag = TransactionCategoryTag.Education,
            MostSpendingDay = new DateTime(2022, 02, 02), BiggestExpense = 100, CreatedAt = new DateTime(2022, 02, 02),
            SumIncome = 100, Categories = new HashSet<TransactionCategoryTag>(),
            SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()

        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Report> actualResult = await ConvertResponseData<List<Report>>(response);
        Assert.Equal(expectedResult, actualResult[0]);
    }
    
    [Theory]
    [InlineData("/report/report/user/3")]
    public async Task Get_ReportsByUserId_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/report/report/account/1")]
    public async Task Get_ReportsByAccountId_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new Report
        {

            Id = 1, AccountId = 1, ReportType = ReportType.Monthly, StartDate = new DateTime(2022, 02, 02),
            EndDate = new DateTime(2022, 02, 02), SumExpense = 100, AverageSpendingDaily = 100,
            AverageSpendingTransaction = 100, MostSpendingTag = TransactionCategoryTag.Education,
            MostSpendingDay = new DateTime(2022, 02, 02), BiggestExpense = 100, CreatedAt = new DateTime(2022, 02, 02),
            SumIncome = 100, Categories = new HashSet<TransactionCategoryTag>(),
            SpendingByTags = new Dictionary<TransactionCategoryTag, decimal>()

        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Report> actualResult = await ConvertResponseData<List<Report>>(response);
        Assert.Equal(expectedResult, actualResult[0]);
    }
    
    [Theory]
    [InlineData("/report/report/account/3")]
    public async Task Get_ReportsByAccountId_Returns_NotFound(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/transaction/transactions")]
    public async Task Get_AllTransactions_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Transaction>()
        {
            new ()
            {
                Id = 1, AccountId = 1, Amount = 1400, Date = new DateTime(2022, 02, 02), Name = "Test",
                Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
            }
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Transaction> actualResult = await ConvertResponseData<List<Transaction>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/transactions/1")]
    public async Task Get_Transaction_By_Id_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new Transaction()
        {
            Id = 1, AccountId = 1, Amount = 1400, Date = new DateTime(2022, 02, 02), Name = "Test",
            Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        Transaction actualResult = await ConvertResponseData<Transaction>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/transactions/3")]
    public async Task Get_Transaction_By_Id_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/transaction/filterByType/Expense")]
    public async Task Get_Transaction_By_Type_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Transaction>()
        {
            new()
            {
                Id = 1, AccountId = 1, Amount = 1400, Date = new DateTime(2022, 02, 02), Name = "Test",
                Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
            }
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Transaction> actualResult = await ConvertResponseData<List<Transaction>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/filterByType/Income")]
    public async Task Get_Transaction_By_Type_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Theory]
    [InlineData("/transaction/filterByTag/Clothing")]
    public async Task Get_Transaction_By_Tag_Returns_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Transaction>()
        {
            new()
            {
                Id = 1, AccountId = 1, Amount = 1400, Date = new DateTime(2022, 02, 02), Name = "Test",
                Tag = TransactionCategoryTag.Clothing, Type = TransactionType.Expense
            }
        };
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Transaction> actualResult = await ConvertResponseData<List<Transaction>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/filterByTag/Education")]
    public async Task Get_Transaction_By_Tag_Returns_NotFound(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var factory = new BudgetBuddyWebApplicationFactory<Program>();
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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