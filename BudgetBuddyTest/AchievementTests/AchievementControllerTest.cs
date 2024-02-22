using BudgetBuddy.Controllers;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Achievement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal;

namespace BudgetBuddyTest.AchievementTests;

[TestFixture]
public class AchievementControllerTest
{
    private Mock<ILogger<AchievementController>> _loggerMock;
    private Mock<IAchievementRepository> _achievementRepositoryMock;
    private AchievementController _controller;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<AchievementController>>();
        _achievementRepositoryMock = new Mock<IAchievementRepository>();
        _controller = new AchievementController(_loggerMock.Object, _achievementRepositoryMock.Object);
    }

    [Test]
    public void GetAll_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.GetAllAchievements()).Throws(new Exception());
        
        var result = _controller.GetAll();
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        
        var objectResult = (NotFoundObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Can't find achievements."));
    }

    [Test]
    public void GetAll_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievements = new List<Achievement> { new Achievement()};
        _achievementRepositoryMock.Setup(x => x.GetAllAchievements()).Returns(achievements);

        var result = _controller.GetAll();
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Achievements found successfully."));
        Assert.That(dataValue, Is.EquivalentTo(achievements));
    }

    [Test]
    public void Get_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.GetAchievement(It.IsAny<int>())).Throws(new Exception());
        
        var result = _controller.Get(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public void Get_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievement = new Achievement();
        _achievementRepositoryMock.Setup(x => x.GetAchievement(It.IsAny<int>())).Returns(achievement);

        var result = _controller.Get(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Achievement found successfully."));
        Assert.That(dataValue, Is.EqualTo(achievement));
    }

    [Test]
    public void Add_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.AddAchievement(It.IsAny<IEnumerable<Achievement>>())).Throws(new Exception());
        
        var result = _controller.Add(It.IsAny<IEnumerable<Achievement>>());
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public void Add_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievements = new List<Achievement>();
        _achievementRepositoryMock.Setup(x => x.AddAchievement(It.IsAny<IEnumerable<Achievement>>())).Returns(achievements);

        var result = _controller.Add(achievements);
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Achievement(s) successfully added."));
        Assert.That(dataValue, Is.EqualTo(achievements));
    }

    [Test]
    public void Delete_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.DeleteAchievement(It.IsAny<int>())).Throws(new Exception());
        
        var result = _controller.Delete(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public void Delete_ReturnsOkIfRepositoryReturnsValidData()
    {
        _achievementRepositoryMock.Setup(x => x.DeleteAchievement(It.IsAny<int>()));

        var result = _controller.Delete(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageProperty = responseData.GetType().GetProperty("message");
        
        Assert.NotNull(messageProperty);

        var messageValue = messageProperty.GetValue(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Deleting achievement was successful."));
    }

    [Test]
    public void Update_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.UpdateAchievement(It.IsAny<Achievement>())).Throws(new Exception());
        
        var result = _controller.Update(It.IsAny<Achievement>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public void Update_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievement = new Achievement();
        _achievementRepositoryMock.Setup(x => x.UpdateAchievement(It.IsAny<Achievement>())).Returns(achievement);

        var result = _controller.Update(achievement);
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Updating message was successful."));
        Assert.That(dataValue, Is.EqualTo(achievement));
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