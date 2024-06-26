using System.Globalization;
using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Controllers;
using BudgetBuddy.Model;
using BudgetBuddy.Services.AchievementService;
using BudgetBuddy.Services.Repositories.Account;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetBuddyTest.AccountTests;

public class AccountControllerTest
{
    private Mock<ILogger<AccountController>> _loggerMock;
    private Mock<IAccountRepository> _accountRepositoryMock;
    private AccountController _controller;
    private Mock<IAchievementService> _achievementServiceMock;
    private Mock<IUserRepository> _userRepositoryMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<AccountController>>();
        _accountRepositoryMock = new Mock<IAccountRepository>();
        _achievementServiceMock = new Mock<IAchievementService>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _controller = new AccountController(_loggerMock.Object, _accountRepositoryMock.Object, _achievementServiceMock.Object, _userRepositoryMock.Object);
    }

    [Test]
    public async Task GetReturnsNotFoundWhenRepositoryThrowsException()
    {
        _accountRepositoryMock.Setup(x => x.GetByUserId(It.IsAny<string>())).Throws(new Exception());
        
        var result = await _controller.Get("1");
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }

    [Test]
    public async Task GetReturnsOkIfRepositoryReturnsValidData()
    {
        var account = new Account {Id = 1, Date = new DateTime(2022, 02, 02), Balance = 5.656m, Name = "sample1", Type = "simple1", UserId = "1", Transactions = new List<Transaction>()};
        _accountRepositoryMock.Setup(x => x.GetByUserId("1")).ReturnsAsync(new List<Account>() { account });
    
        var result = await _controller.Get("1");
        
        Assert.IsInstanceOf<ActionResult<List<Account>>>(result);

        var objectResult = result.Result;
        Assert.IsInstanceOf<OkObjectResult>(objectResult);

        var okObjectResult = (OkObjectResult)objectResult;
        var responseData = okObjectResult.Value;
    
        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Account retrieved successfully"));
        Assert.That(dataValue, Is.EqualTo(new List<Account> { account }));
    }
    
    
    [Test]
    public async Task CreateAccountReturnsNotFoundWhenRepositoryThrowsException()
    {
        _accountRepositoryMock.Setup(x => x.CreateAccount(It.IsAny<AccountCreateRequest>())).Throws(new Exception());
        
        var result = await _controller.CreateAccount(It.IsAny<AccountCreateRequest>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public async Task CreateAccountReturnsExceptionWhenAchievementSystemUserNotFound()
    {
        _accountRepositoryMock.Setup(x => x.CreateAccount(It.IsAny<AccountCreateRequest>())).ReturnsAsync(new Account());
        _userRepositoryMock.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
        var result = await _controller.CreateAccount(It.IsAny<AccountCreateRequest>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public async Task CreateAccountReturnsOkIfRepositoryReturnsValidData()
    {
        var accountRequest = new AccountCreateRequest(100, "A", "A", "1");
        var account = new Account {Id = 1, Balance = 5.656m, Name = "A", Type = "A", UserId = "1", Transactions = new List<Transaction>()};
        _accountRepositoryMock.Setup(x => x.CreateAccount(It.IsAny<AccountCreateRequest>())).ReturnsAsync(account);
        _userRepositoryMock.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
    
        var result = await _controller.CreateAccount(accountRequest);
        Assert.IsInstanceOf<ActionResult<Account>>(result);
    
        var objectResult = result.Result;
        Assert.IsInstanceOf<OkObjectResult>(objectResult);
        var okObjectResult = (OkObjectResult)objectResult;
        
        var responseData = okObjectResult.Value;
    
        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Account created successfully"));
        Assert.That(dataValue, Is.EqualTo(account));
    }
    
    [Test]
    public async Task Delete_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _accountRepositoryMock.Setup(x => x.DeleteAccount(It.IsAny<int>())).Throws(new Exception());
        
        var result = await _controller.DeleteAccount(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public async Task DeleteReturnsOkIfRepositoryReturnsValidData()
    {
        _accountRepositoryMock.Setup(x => x.DeleteAccount(It.IsAny<int>()));
        
        var result = await _controller.DeleteAccount(It.IsAny<int>());
        
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        
        var okResult = (OkObjectResult)result.Result;
        Assert.AreEqual(200, okResult.StatusCode);
        
        var responseData = okResult.Value;
        var messageProperty = responseData.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        
        var messageValue = messageProperty.GetValue(responseData);
        Assert.That(messageValue, Is.EqualTo("Account deleted successfully"));
    }
    
    [Test]
    public async Task UpdateReturnsNotFoundWhenRepositoryThrowsException()
    {
        _accountRepositoryMock.Setup(x => x.UpdateAccount(It.IsAny<AccountUpdateRequest>())).Throws(new Exception());
        
        var result = await _controller.UpdateAccount(It.IsAny<AccountUpdateRequest>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public async Task Update_ReturnsOkIfRepositoryReturnsValidData()
    {
        var accountUpdate = new AccountUpdateRequest(0, 150, "B", "A", "1");
        var account = new Account {Id = 0, Balance = 150, Name = "B", Type = "A", UserId = "1", Transactions = new List<Transaction>()};
        _accountRepositoryMock.Setup(x => x.UpdateAccount(It.IsAny<AccountUpdateRequest>())).ReturnsAsync(account);
    
        var result = await _controller.UpdateAccount(accountUpdate);
        Assert.IsInstanceOf<ActionResult<Account>>(result);
    
        var objectResult = result;
        Assert.IsInstanceOf<OkObjectResult>(objectResult.Result);
        var okObjectResult = (OkObjectResult)objectResult.Result;
        
        var responseData = okObjectResult.Value;
    
        var messageValue = GetMessageFromResult(responseData);
        var dataValue = (Account)GetDataFromResult(responseData);
        Assert.That(messageValue, Is.EqualTo("Account updated successfully"));
        Assert.That(dataValue, Is.EqualTo(account));
    }
    
    private object GetMessageFromResult(object responseData)
    {
        var messageProperty = responseData.GetType().GetProperty("message");
        return messageProperty.GetValue(responseData);
    }
    
    private object GetDataFromResult(object responseData)
    {
        var dataProperty = responseData.GetType().GetProperty("data");
        return dataProperty.GetValue(responseData);
    }
}
