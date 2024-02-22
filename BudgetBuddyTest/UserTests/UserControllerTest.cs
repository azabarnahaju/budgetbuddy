using BudgetBuddy.Controllers;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Authentication;
using BudgetBuddy.Services.Repositories.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;

namespace BudgetBuddyTest.UserTests;

public class UserControllerTest
{
    private Mock<ILogger<UserController>> _loggerMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IAuthenticationService> _authenticationService;
    private UserController _controller;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<UserController>>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _authenticationService = new Mock<IAuthenticationService>();
        _controller = new UserController(_loggerMock.Object, _userRepositoryMock.Object, _authenticationService.Object);
    }

    [Test]
    public void Get_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<int>())).Throws(new Exception());

        var result = _controller.Get(It.IsAny<int>());

        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public void Get_ReturnsOkIfRepositoryReturnsValidData()
    {
        var user = new User();
        _userRepositoryMock.Setup(x => x.GetUser(It.IsAny<int>())).Returns(user);

        var result = _controller.Get(It.IsAny<int>());

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);

        Assert.That(messageValue, Is.EqualTo("User data successfully retrieved."));
        Assert.That(dataValue, Is.EqualTo(user));
    }
    
    [Test]
    public void Update_ReturnsBadRequestWhenRepositoryThrowsException()
    {
        _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Throws(new Exception());

        var result = _controller.Update(It.IsAny<User>());

        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public void Update_ReturnsOkIfRepositoryReturnsValidData()
    {
        var user = new User();
        _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Returns(user);

        var result = _controller.Update(It.IsAny<User>());

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);

        Assert.That(messageValue, Is.EqualTo("Updating user was successful."));
        Assert.That(dataValue, Is.EqualTo(user));
    }
    
    [Test]
    public void Delete_ReturnsBadRequestWhenRepositoryThrowsException()
    {
        _userRepositoryMock.Setup(x => x.DeleteUser(It.IsAny<int>())).Throws(new Exception());

        var result = _controller.Delete(It.IsAny<int>());

        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public void Delete_ReturnsOkIfRepositoryProcessWasSuccessful()
    {
        _userRepositoryMock.Setup(x => x.DeleteUser(It.IsAny<int>()));

        var result = _controller.Delete(It.IsAny<int>());

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);

        Assert.That(messageValue, Is.EqualTo("User deleted successfully."));
    }
    
    [Test]
    public void Register_ReturnsBadRequestWhenRepositoryThrowsException()
    {
        _userRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>())).Throws(new Exception());

        var result = _controller.Register(It.IsAny<User>());

        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public void Register_ReturnsOkIfRepositoryReturnsValidData()
    {
        var user = new User();
        _userRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>())).Returns(user);

        var result = _controller.Register(It.IsAny<User>());

        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Registration successful."));
        Assert.That(dataValue, Is.EqualTo(user));
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
