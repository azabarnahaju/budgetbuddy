using BudgetBuddy.Controllers;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums;
using BudgetBuddy.Services.Repositories.Transaction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetBuddyTest.TransactionTests;

[TestFixture]
public class TransactionControllerTest
{
    private Mock<ILogger<TransactionController>> _loggerMock;
    private Mock<ITransactionRepository> _transactionDataProviderMock;
    private TransactionController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<TransactionController>>();
        _transactionDataProviderMock = new Mock<ITransactionRepository>();
        _controller = new TransactionController(_loggerMock.Object, _transactionDataProviderMock.Object);
    }
    
    //AddTransaction
    
    [Test]
    public void AddTransaction_ReturnsNotFoundResultIfTransactionDataProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.AddTransaction(It.IsAny<Transaction>())).Throws(new Exception());
        
        // Act
        var result = _controller.AddTransaction(It.IsAny<Transaction>());
        var resultMessage = (BadRequestObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction already exists."));
    }
    
    [Test]
    public void AddTransaction_ReturnsOkResultIfTransactionDataIsValid()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.AddTransaction(It.IsAny<Transaction>()));
        
        // Act
        var result = _controller.AddTransaction(It.IsAny<Transaction>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction added."));
    }
    
    //GetAll
    
    [Test]
    public void GetAll_ReturnsNotFoundResultIfTransactionDataProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.GetAllTransactions()).Throws(new Exception());
        
        // Act
        var result = _controller.GetAll();
        var resultMessage = (BadRequestObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Error getting transactions"));
    }
    
    
    [Test]
    public void GetAll_ReturnsOkResultIfTransactionDataIsValid()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.GetAllTransactions()).Returns(new List<Transaction>());
        
        // Act
        var result = _controller.GetAll();
        var resultMessage = (OkObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transactions retrieved."));
    }
    
    //GetTransaction

    [Test]
    public void GetTransaction_ReturnsNotFoundResultIfTransactionDataProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.GetTransaction(It.IsAny<int>())).Throws(new Exception());
        
        // Act
        var result = _controller.GetTransaction(It.IsAny<int>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        // Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction not found."));
    }
    
    [Test]
    public void GetTransaction_ReturnsOkResultIfTransactionDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.GetTransaction(It.IsAny<int>())).Returns(It.IsAny<Transaction>());
        
        //Act
        var result = _controller.GetTransaction(It.IsAny<int>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction retrieved."));
    }
    
    //UpdateTransaction

    [Test]
    public void UpdateTransaction_ReturnsNotFoundIfProviderFails()
    {
        // Arrange
        _transactionDataProviderMock.Setup(x => x.UpdateTransaction(It.IsAny<Transaction>())).Throws(
            new Exception());
        
        // Act
        var result = _controller.UpdateTransaction(It.IsAny<Transaction>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
    
        // Assert
        Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Error updating transaction"));
    }

    [Test]
    public void UpdateTransaction_ReturnsOkResultIfTransactionDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.UpdateTransaction(It.IsAny<Transaction>()));
        
        //Act
        var result = _controller.UpdateTransaction(It.IsAny<Transaction>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo("Transaction updated."));
    }
    
    //DeleteTransaction

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
    
    //FilterByType
    [Test]
    public void FilterTransactions_ReturnsNotFoundIfProviderFails()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FilterTransactions(It.IsAny<TransactionType>())).Throws(new Exception());
        
        //Act
        var result = _controller.FilterTransactions(It.IsAny<TransactionType>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Error filtering transactions by { It.IsAny<TransactionType>() }"));
    }

    [Test]
    public void FilterTransactions_ReturnsOkIfTransactionsDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FilterTransactions(It.IsAny<TransactionType>()))
            .Returns(new List<Transaction>());
        
        //Act
        var result = _controller.FilterTransactions(It.IsAny<TransactionType>());
        var resultMessage = (OkObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Transactions filtered by { It.IsAny<TransactionType>() }."));
    }
    
    //FinancialTransactions
    [Test]
    public void FinancialTransactions_ReturnsNotFoundIfProviderFails()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FinancialTransactions(It.IsAny<TransactionCategoryTag>())).Throws(
            new Exception());
        
        //Act
        var result = _controller.FinancialTransactions(It.IsAny<TransactionCategoryTag>());
        var resultMessage = (NotFoundObjectResult?)result.Result;
        
        //Assert
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        Assert.That(GetMessageFromResult(resultMessage.Value), Is.EqualTo($"Error filtering transactions by { It.IsAny<TransactionCategoryTag>() } tag."));
    }

    [Test]
    public void FinancialTransactions_ReturnsOkIfFinancialTransactionsDataIsValid()
    {
        //Arrange
        _transactionDataProviderMock.Setup(x => x.FinancialTransactions(It.IsAny<TransactionCategoryTag>()))
            .Returns(new List<Transaction>());
        
        //Act
        var result = _controller.FinancialTransactions(It.IsAny<TransactionCategoryTag>());
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