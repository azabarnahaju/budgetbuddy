using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Controllers;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Model.Enums.TransactionEnums;
using BudgetBuddy.Services.Repositories.Achievement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

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
    public async Task GetAll_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.GetAllAchievements()).Throws(new Exception());
        
        var result = await _controller.GetAll();
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
        
        var objectResult = (NotFoundObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Can't find achievements."));
    }

    [Test]
    public async Task GetAll_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievements = new List<Achievement> { new Achievement("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account)};
        _achievementRepositoryMock.Setup(x => x.GetAllAchievements()).ReturnsAsync(achievements);

        var result = await _controller.GetAll();
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Achievements found successfully."));
        Assert.That(dataValue, Is.EquivalentTo(achievements));
    }

    [Test]
    public async Task Get_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.GetAchievement(It.IsAny<int>())).Throws(new Exception());
        
        var result = await _controller.Get(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task Get_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievement = new Achievement("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account);
        _achievementRepositoryMock.Setup(x => x.GetAchievement(It.IsAny<int>())).ReturnsAsync(achievement);

        var result = await _controller.Get(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Achievement found successfully."));
        Assert.That(dataValue, Is.EqualTo(achievement));
    }

    [Test]
    public async Task Add_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.AddAchievement(It.IsAny<IEnumerable<AchievementCreateRequest>>())).Throws(new Exception());
        
        var result = await _controller.Add(It.IsAny<IEnumerable<AchievementCreateRequest>>());
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result.Result);
    }
    
    [Test]
    public async Task Add_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievements = new List<Achievement>();
        _achievementRepositoryMock.Setup(x => x.AddAchievement(It.IsAny<IEnumerable<AchievementCreateRequest>>())).ReturnsAsync(achievements);
        var achievementsToCreate = new List<AchievementCreateRequest>();
        var result = await _controller.Add(achievementsToCreate);
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageValue = GetMessageFromResult(responseData);
        var dataValue = GetDataFromResult(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Achievement(s) successfully added."));
        Assert.That(dataValue, Is.EqualTo(achievements));
    }

    [Test]
    public async Task Delete_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.DeleteAchievement(It.IsAny<int>())).Throws(new Exception());
        
        var result = await _controller.Delete(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public async Task Delete_ReturnsOkIfRepositoryReturnsValidData()
    {
        _achievementRepositoryMock.Setup(x => x.DeleteAchievement(It.IsAny<int>()));

        var result = await _controller.Delete(It.IsAny<int>());
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result.Result);

        var objectResult = (OkObjectResult)result.Result;
        var responseData = objectResult.Value;

        var messageProperty = responseData.GetType().GetProperty("message");
        
        Assert.NotNull(messageProperty);

        var messageValue = messageProperty.GetValue(responseData);
        
        Assert.That(messageValue, Is.EqualTo("Deleting achievement was successful."));
    }

    [Test]
    public async Task Update_ReturnsNotFoundWhenRepositoryThrowsException()
    {
        _achievementRepositoryMock.Setup(x => x.UpdateAchievement(It.IsAny<AchievementUpdateRequest>())).Throws(new Exception());
        
        var result = await _controller.Update(It.IsAny<AchievementUpdateRequest>());
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result.Result);
    }
    
    [Test]
    public async Task Update_ReturnsOkIfRepositoryReturnsValidData()
    {
        var achievement = new Achievement("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account){ Id = 1};
        _achievementRepositoryMock.Setup(x => x.UpdateAchievement(It.IsAny<AchievementUpdateRequest>())).ReturnsAsync(achievement);
        var achievementToUpdate = new AchievementUpdateRequest(1, "Test", "Test");
        var result = await _controller.Update(achievementToUpdate);
        
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