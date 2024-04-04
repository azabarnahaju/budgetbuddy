using System.Net;
using System.Net.Http.Headers;
using BudgetBuddy.IntegrationTests.JwtAuthenticationTest;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using FluentAssertions;
using Newtonsoft.Json;

namespace BudgetBuddy.IntegrationTests.IntegrationTests;

public class BasicTests : IClassFixture<BudgetBuddyWebApplicationFactory<Program>>
{
    private readonly BudgetBuddyWebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public BasicTests(BudgetBuddyWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Theory]
    [InlineData("/account/385")]
    [InlineData("/achievement")]
    [InlineData("/achievement/1")]
    [InlineData("/goal/1")]
    [InlineData("/report")]
    [InlineData("/report/1")]
    [InlineData("/report/report/user/385")]
    [InlineData("/report/report/account/1")]
    [InlineData("/transaction/transactions")]
    [InlineData("/transaction/transactions/1")]
    [InlineData("/transaction/filterByType/testtype")]
    [InlineData("/transaction/filterByTag/testtag")]
    public async Task Get_Should_Reject_Unauthenticated_Requests(string url)
    {
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Theory]
    [InlineData("/account/385")]
    [InlineData("/achievement")]
    [InlineData("/achievement/1")]
    [InlineData("/goal/1")]
    [InlineData("/report/1")]
    [InlineData("/report/report/user/385")]
    [InlineData("/report/report/account/1")]
    [InlineData("/transaction/transactions")]
    [InlineData("/transaction/transactions/1")]
    [InlineData("/transaction/filterByType/Expense")]
    [InlineData("/transaction/filterByTag/Clothing")]
    public async Task Get_Should_Allow_All_Registered_Users(string url)
    {
        var token = new TestJwtToken().WithRole("User").WithName("testuser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("/account/385")]
    [InlineData("/achievement")]
    [InlineData("/achievement/1")]
    [InlineData("/goal/1")]
    [InlineData("/report/1")]
    [InlineData("/report")]
    [InlineData("/report/report/user/385")]
    [InlineData("/report/report/account/1")]
    [InlineData("/transaction/transactions")]
    [InlineData("/transaction/transactions/1")]
    [InlineData("/transaction/filterByType/Expense")]
    [InlineData("/transaction/filterByTag/Clothing")]
    public async Task Get_Should_Allow_All_Registered_Admins(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("/account/385")]
    public async Task Get_Accounts_By_UserId_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Account>()
        {
            new ()
            {
                Id = 1, UserId = "385", Date = new DateTime(2022, 02, 02), Balance = 1500, Name = "Test", Type = "Test"
            }
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Account> actualResult = await ConvertResponseData<List<Account>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/achievement")]
    public async Task Get_AllAchievements_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new List<Achievement>()
        {
            new () { Id = 1, Description = "Test", Name = "Test" }
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Achievement> actualResult = await ConvertResponseData<List<Achievement>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/achievement/1")]
    public async Task Get_AchievementById_Return_Correct_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        var expectedResult = new Achievement() { Id = 1, Description = "Test", Name = "Test" };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        Achievement actualResult = await ConvertResponseData<Achievement>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/achievement/3")]
    public async Task Get_AchievementById_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
                AccountId = 1, UserId = "385", Id = 1, Completed = false, CurrentProgress = 0,
                StartDate = new DateTime(2022, 02, 02),
                Type = GoalType.Income, Target = 100
            }
        };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Goal> actualResult = await ConvertResponseData<List<Goal>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/goal/3")]
    public async Task Get_GoalByAccountId_Returns_Empty_Object(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var expected = new List<Goal>();
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Goal> actualResult = await ConvertResponseData<List<Goal>>(response);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        Report actualResult = await ConvertResponseData<Report>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/report/3")]
    public async Task Get_ReportById_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Report> actualResult = await ConvertResponseData<List<Report>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/report/report/user/385")]
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Report> actualResult = await ConvertResponseData<List<Report>>(response);
        Assert.Equal(expectedResult, actualResult[0]);
    }
    
    [Theory]
    [InlineData("/report/report/user/3")]
    public async Task Get_ReportsByUserId_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Report> actualResult = await ConvertResponseData<List<Report>>(response);
        Assert.Equal(expectedResult, actualResult[0]);
    }
    
    [Theory]
    [InlineData("/report/report/account/3")]
    public async Task Get_ReportsByAccountId_Returns_NotFound(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        Transaction actualResult = await ConvertResponseData<Transaction>(response);
        Assert.Equal(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/transactions/3")]
    public async Task Get_Transaction_By_Id_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Transaction> actualResult = await ConvertResponseData<List<Transaction>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/filterByType/Income")]
    public async Task Get_Transaction_By_Type_Returns_Not_Found(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        List<Transaction> actualResult = await ConvertResponseData<List<Transaction>>(response);
        Assert.Equivalent(expectedResult, actualResult);
    }
    
    [Theory]
    [InlineData("/transaction/filterByTag/Education")]
    public async Task Get_Transaction_By_Tag_Returns_NotFound(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testadmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
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