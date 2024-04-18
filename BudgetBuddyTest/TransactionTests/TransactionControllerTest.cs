using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Controllers;
using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.AchievementService;
using BudgetBuddy.Services.GoalServices;
using BudgetBuddy.Services.Repositories.Transaction;
using BudgetBuddy.Services.TransactionServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetBuddyTest.TransactionTests;

[TestFixture]
public class TransactionControllerTest
{
    private Mock<ILogger<TransactionController>> _loggerMock;
    private Mock<ITransactionRepository> _transactionDataProviderMock;
    private Mock<IGoalService> _goalServiceMock;
    private Mock<ITransactionService> _transactionServiceMock;
    private Mock<IAchievementService> _achievementServiceMock;
    private DbContextOptions<BudgetBuddyContext> _contextOptions;
    private BudgetBuddyContext _dbContext;
    private TransactionController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<TransactionController>>();
        _transactionDataProviderMock = new Mock<ITransactionRepository>();
        _goalServiceMock = new Mock<IGoalService>();
        _transactionServiceMock = new Mock<ITransactionService>();
        _achievementServiceMock = new Mock<IAchievementService>();
        
        _contextOptions = new DbContextOptionsBuilder<BudgetBuddyContext>()
            .UseInMemoryDatabase("BloggingControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        _dbContext = new BudgetBuddyContext(_contextOptions);

        _controller = new TransactionController(_loggerMock.Object, _transactionDataProviderMock.Object,
            _goalServiceMock.Object, _transactionServiceMock.Object, _achievementServiceMock.Object,
            _dbContext);
    }
    
    //AddTransaction
    
    [Test]
    public async Task AddTransaction_ReturnsNotFoundResultIfTransactionDataProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.AddTransaction(It.IsAny<TransactionCreateRequest>())).Throws(new Exception());
        
        // Act
        var result = await _controller.AddTransaction(It.IsAny<TransactionCreateRequest>());
        var resultMessage = (BadRequestObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction already exists."));
    }
    
    [Test]
    public async Task AddTransaction_ReturnsOkResultIfTransactionDataIsValid()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.AddTransaction(It.IsAny<TransactionCreateRequest>())).ReturnsAsync(It.IsAny<Transaction>());
        
        // Act
        var result = await _controller.AddTransaction(It.IsAny<TransactionCreateRequest>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction added."));
    }
    
    // //GetAll
    
    [Test]
    public async Task GetAll_ReturnsNotFoundResultIfTransactionDataProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.GetAllTransactions()).Throws(new Exception());
        
        // Act
        var result = await _controller.GetAll();
        var resultMessage = (BadRequestObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Error getting transactions"));
    }
    
    
    [Test]
    public async Task GetAll_ReturnsOkResultIfTransactionDataIsValid()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.GetAllTransactions()).ReturnsAsync(new List<Transaction>());
        
        // Act
        var result = await _controller.GetAll();
        var resultMessage = (OkObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transactions retrieved."));
        Assert.That(GetDataFromResult(resultMessage.Value), Is.EqualTo(new List<Transaction>()));
    }
    
    // //GetTransaction
    
    [Test]
    public async Task GetTransaction_ReturnsNotFoundResultIfTransactionDataProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.GetTransaction(It.IsAny<int>())).Throws(new Exception());
        
        // Act
        var result = await _controller.GetTransaction(It.IsAny<int>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction not found."));
    }
    
    [Test]
    public async Task GetTransaction_ReturnsOkResultIfTransactionDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.GetTransaction(It.IsAny<int>())).ReturnsAsync(It.IsAny<Transaction>());
        
        //Act
        var result = await _controller.GetTransaction(It.IsAny<int>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction retrieved."));
    }
    
    // //UpdateTransaction
    
    [Test]
    public async Task UpdateTransaction_ReturnsNotFoundIfProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.UpdateTransaction(It.IsAny<TransactionUpdateRequest>())).Throws(
            new Exception());
        
        // Act
        var result = await _controller.UpdateTransaction(It.IsAny<TransactionUpdateRequest>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
    
        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Error updating transaction"));
    }
    
    [Test]
    public async Task UpdateTransaction_ReturnsOkResultIfTransactionDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.UpdateTransaction(It.IsAny<TransactionUpdateRequest>()));
        
        //Act
        var result = await _controller.UpdateTransaction(It.IsAny<TransactionUpdateRequest>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction updated."));
        Assert.That(GetDataFromResult(resultMessage.Value), Is.EqualTo(It.IsAny<Transaction>()));
    }
    
    // //DeleteTransaction
    
    [Test]
    public void DeleteTransaction_ReturnsNotFoundIfProviderFails()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.DeleteTransaction(It.IsAny<int>())).Throws(new Exception());
        
        //Act
        var result = _controller.DeleteTransaction(It.IsAny<int>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Error deleting transaction"));
    }
    
    [Test]
    public void DeleteTransaction_ReturnsOkIfDeleteTransactionDataInvalid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.DeleteTransaction(It.IsAny<int>()));
        
        //Act
        var result = _controller.DeleteTransaction(It.IsAny<int>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction deleted."));
    }
    
    // //FilterByType
    
    [Test]
    public async Task FilterTransactions_ReturnsNotFoundIfProviderFails()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FilterTransactions(It.IsAny<TransactionType>())).Throws(new Exception());
        
        //Act
        var result = await _controller.FilterTransactions(It.IsAny<TransactionType>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Error filtering transactions by { It.IsAny<TransactionType>() }"));
    }
    
    [Test]
    public async Task FilterTransactions_ReturnsOkIfTransactionsDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FilterTransactions(It.IsAny<TransactionType>()))
            .ReturnsAsync(new List<Transaction>());
        
        //Act
        var result = await _controller.FilterTransactions(It.IsAny<TransactionType>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Transactions filtered by { It.IsAny<TransactionType>() }."));
        Assert.That(GetDataFromResult(resultMessage.Value), Is.EqualTo(new List<Transaction>()));
    }
    
    // //FinancialTransactions
    
    [Test]
    public async Task FinancialTransactions_ReturnsNotFoundIfProviderFails()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FinancialTransactions(It.IsAny<TransactionCategoryTag>())).Throws(
            new Exception());
        
        //Act
        var result = await _controller.FinancialTransactions(It.IsAny<TransactionCategoryTag>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Error filtering transactions by { It.IsAny<TransactionCategoryTag>() } tag."));
    }
    
    [Test]
    public async Task FinancialTransactions_ReturnsOkIfFinancialTransactionsDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FinancialTransactions(It.IsAny<TransactionCategoryTag>()))
            .ReturnsAsync(new List<Transaction>());
        
        //Act
        var result = await _controller.FinancialTransactions(It.IsAny<TransactionCategoryTag>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Transactions filtered by { It.IsAny<TransactionCategoryTag>() }."));
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
