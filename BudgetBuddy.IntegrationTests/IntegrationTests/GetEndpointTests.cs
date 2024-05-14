using System.Net;
using System.Net.Http.Headers;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit.Abstractions;

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
                new Achievement("Pioneer", AchievementType.Exploration, 1, AchievementObjectiveType.Transaction) { Id = 1, Users = new HashSet<ApplicationUser>()},
                new Achievement("Money Tracker", AchievementType.Exploration, 10, AchievementObjectiveType.Transaction){ Id = 2,Users = new HashSet<ApplicationUser>()},
                new Achievement("Transaction Pro", AchievementType.Exploration, 25, AchievementObjectiveType.Transaction) { Id = 3,Users = new HashSet<ApplicationUser>() },
                new Achievement("Account Starter", AchievementType.Exploration, 1, AchievementObjectiveType.Account){ Id = 4,Users = new HashSet<ApplicationUser>() },
                new Achievement("Multi-Account Holder", AchievementType.Exploration, 3, AchievementObjectiveType.Account){ Id = 5,Users = new HashSet<ApplicationUser>() },
                new Achievement("Master of Accounts", AchievementType.Exploration, 5, AchievementObjectiveType.Account){ Id = 6,Users = new HashSet<ApplicationUser>() },
                new Achievement("Income Beginner", AchievementType.Exploration, 1, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 7,Users = new HashSet<ApplicationUser>() },
                new Achievement("Income Earner", AchievementType.Exploration, 10, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 8,Users = new HashSet<ApplicationUser>() },
                new Achievement("Income Guru", AchievementType.Exploration, 25, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 9,Users = new HashSet<ApplicationUser>() },
                new Achievement("Master of Income", AchievementType.Exploration, 50, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 10,Users = new HashSet<ApplicationUser>() },
                new Achievement("Expense Beginner", AchievementType.Exploration, 1, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 11,Users = new HashSet<ApplicationUser>() },
                new Achievement("Expense Proficient", AchievementType.Exploration, 10, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 12,Users = new HashSet<ApplicationUser>() },
                new Achievement("Big Spender", AchievementType.Exploration, 25, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 13,Users = new HashSet<ApplicationUser>() },
                new Achievement("Master of Expenses", AchievementType.Exploration, 50, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 14,Users = new HashSet<ApplicationUser>() },
                new Achievement ("Goal Getter", AchievementType.Exploration, 1, AchievementObjectiveType.Goal){ Id = 15,Users = new HashSet<ApplicationUser>() },
                new Achievement ("Goal Digger", AchievementType.Exploration, 3, AchievementObjectiveType.Goal){ Id = 16,Users = new HashSet<ApplicationUser>() },
                new Achievement ("Goal Crusher", AchievementType.Exploration, 5, AchievementObjectiveType.Goal){ Id = 17,Users = new HashSet<ApplicationUser>() },
                new Achievement("Penny Pincher", AchievementType.AmountBased, 100, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 18,Users = new HashSet<ApplicationUser>() },
                new Achievement("Budget Boss", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 19,Users = new HashSet<ApplicationUser>() },
                new Achievement("Financial Guru", AchievementType.AmountBased, 2000, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 20,Users = new HashSet<ApplicationUser>() },
                new Achievement("Money Master", AchievementType.AmountBased, 5000, AchievementObjectiveType.TransactionType, TransactionType.Expense){ Id = 21,Users = new HashSet<ApplicationUser>() },
                new Achievement("Money Maker", AchievementType.AmountBased, 100, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 22,Users = new HashSet<ApplicationUser>() },
                new Achievement("Cash Flow Captain", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 23,Users = new HashSet<ApplicationUser>() },
                new Achievement("Income Innovator", AchievementType.AmountBased, 2000, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 24,Users = new HashSet<ApplicationUser>() },
                new Achievement("Wealth Wizard", AchievementType.AmountBased, 5000, AchievementObjectiveType.TransactionType, TransactionType.Income){ Id = 25,Users = new HashSet<ApplicationUser>() },
                new Achievement("Entertainment Explorer", AchievementType.AmountBased, 50, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment){ Id = 26,Users = new HashSet<ApplicationUser>() },
                new Achievement("Leisure Luminary", AchievementType.AmountBased, 150, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment){ Id = 27,Users = new HashSet<ApplicationUser>() },
                new Achievement("Fun Fund Fancier", AchievementType.AmountBased, 500, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment){ Id = 28,Users = new HashSet<ApplicationUser>() },
                new Achievement("Entertainment Enthusiast", AchievementType.AmountBased, 1000, AchievementObjectiveType.TransactionTag, null, TransactionCategoryTag.Entertainment){ Id = 29,Users = new HashSet<ApplicationUser>() },
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
        var expectedResult =
            new Achievement("Pioneer", AchievementType.Exploration, 1, AchievementObjectiveType.Transaction)
                { Id = 1, Description = $"You have created 1 Transaction!", Users = new HashSet<ApplicationUser>()};
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