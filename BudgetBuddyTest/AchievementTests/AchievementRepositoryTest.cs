using BudgetBuddy.Contracts.ModelRequest;
using BudgetBuddy.Contracts.ModelRequest.CreateModels;
using BudgetBuddy.Contracts.ModelRequest.UpdateModels;
using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Model.Enums.AchievementEnums;
using BudgetBuddy.Services.Repositories.Achievement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BudgetBuddyTest.AchievementTests;

[TestFixture]
public class AchievementRepositoryTest
{
    private DbContextOptions<BudgetBuddyContext> _contextOptions;
    private BudgetBuddyContext _context;
    private IAchievementRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _contextOptions = new DbContextOptionsBuilder<BudgetBuddyContext>()
            .UseInMemoryDatabase("Achievements")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        
        _context = new BudgetBuddyContext(_contextOptions);
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _context.SaveChanges();
        
        _repository = new AchievementRepository(_context);
    }
    
    [Test]
    public async Task GetAllAchievements_ReturnsEmptyArrayWhenNoAchievements()
    {
        var expectedResult = Array.Empty<Achievement>();
        var actualResult = await _repository.GetAllAchievements();
        
        Assert.That(actualResult, Is.EquivalentTo(expectedResult));
    }
    

    [Test]
    public async Task GetAllAchievements_ReturnsOneWhenHasOneAchievement()
    {
        await _context.AddAsync(new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 });
        await _context.SaveChangesAsync();
        
        var expectedResult = new Achievement[] { new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 } };
        var actualResult = await _repository.GetAllAchievements();
    
        Console.WriteLine($"ID: {expectedResult[0].Id}, Title: {expectedResult[0].Name}, Desc: {expectedResult[0].Description}");
        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Count(), Is.EqualTo(1));
            Assert.That(actualResult, Is.EquivalentTo(expectedResult));
        });
    }
    
    [Test]
    public async Task GetAllAchievements_ReturnsMultipleWhenHasMultipleAchievement()
    {
        var achievements = new List<Achievement>
        {
            new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 },
            new Achievement ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account) { Id = 2 },
            new Achievement ("Test3", AchievementType.Exploration, 5, AchievementObjectiveType.Account) { Id = 3 }
        };
         await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
         
        var expectedResult = achievements;
        var actualResult = await _repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Count, Is.EqualTo(achievements.Count));
            Assert.That(actualResult, Is.EquivalentTo(expectedResult));
        });
    }
    
    [Test]
    public async Task GetAchievement_ReturnsExceptionWhenIdIncorrect()
    {
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.GetAchievement(1));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo("Achievement could not be found."));
    }
    
    [Test]
    public async Task GetAchievement_ReturnsCorrectAchievementForId()
    {
        var achievement = new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 };
        await _context.AddAsync(achievement);
        await _context.SaveChangesAsync();
        
        var actualResult = await _repository.GetAchievement(1);
        
        Assert.That(actualResult, Is.EqualTo(achievement));
    }
    
    [Test]
    public async Task AddAchievement_ThrowExceptionWhenAddingDuplicateAchievements()
    {
        var achievementsToAdd = new List<AchievementCreateRequest>
        {
            new ("Test1", AchievementType.Exploration, 1, AchievementObjectiveType.Account),
            new ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account)
        };
        var result = await _repository.AddAchievement(achievementsToAdd);
        foreach (var VARIABLE in result)
        {
            Console.WriteLine(VARIABLE);
        }
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.AddAchievement(achievementsToAdd));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"You're trying to add duplicate achievements."));
    }
    
    [Test]
    public async Task AddAchievement_ReturnsCorrectAchievementsWhenAddedOne()
    {
        var achievements = new List<Achievement>
        {
            new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 },
            new Achievement ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account) { Id = 2 },
        };
        var expectedAchievements = new List<Achievement>
        {
            new Achievement ("Test3", AchievementType.Exploration, 5, AchievementObjectiveType.Account) { Id = 3, Users = new HashSet<ApplicationUser>() }
        };
        var achievementToAdd = new List<AchievementCreateRequest>
        {
            new AchievementCreateRequest("Test3", AchievementType.Exploration, 5, AchievementObjectiveType.Account),
        };
        
        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
        
        var actualResult = await _repository.AddAchievement(achievementToAdd);
        var currentRepository = await _repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(currentRepository.Count(), Is.EqualTo(achievements.Count + achievementToAdd.Count()));
            Assert.That(actualResult.Count(), Is.EqualTo(achievementToAdd.Count()));
            Assert.That(actualResult, Is.EqualTo(expectedAchievements));
        });
    }
    
    
    [Test]
    public async Task DeleteAchievement_ThrowsExceptionWhenAchievementRepositoryEmpty()
    {
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.DeleteAchievement(1));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"Achievement is not found."));
    }
    
    [Test]
    public async Task DeleteAchievement_ThrowsExceptionWhenIdNotExist()
    {
        var achievements = new List<Achievement>
        {
            new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 },
            new Achievement ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account) { Id = 2 },
        };

        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
        
        var currentRepository = await _repository.GetAllAchievements();
        
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.DeleteAchievement(3));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"Achievement is not found."));
        
        Assert.Multiple(() =>
        {
            Assert.That(achievements.Count, Is.EqualTo(currentRepository.Count()));
            Assert.That(achievements.ToList(), Is.EquivalentTo(currentRepository));
        });
    }
    
    [Test]
    public async Task DeleteAchievement_DeletesAchievementFromRepository()
    {
        var achievements = new List<Achievement>
        {
            new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 },
            new Achievement ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account) { Id = 2 },
        };

        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
        
        var repositoryBefore = await _repository.GetAllAchievements();
        await _repository.DeleteAchievement(1);
        var repositoryAfter = await _repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(repositoryAfter.Count(), Is.EqualTo(achievements.Count() - 1));
            Assert.That(repositoryAfter.Count(), Is.EqualTo(repositoryBefore.Count() - 1));
            Assert.That(repositoryAfter, Does.Not.Contain(new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 }));
        });
    }
    
    [Test]
    public async Task UpdateAchievement_ThrowsExceptionWhenAchievementNotExist()
    {
        var achievements = new List<Achievement>
        {
            new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 },
            new Achievement ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account) { Id = 2 },
        };

        var achievementToUpdate = new AchievementUpdateRequest(3, "TestUpdated", AchievementType.Exploration, 2, AchievementObjectiveType.Goal, "TestDescription");
        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();

        var ex = Assert.ThrowsAsync<Exception>(() => _repository.UpdateAchievement(achievementToUpdate));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"Achievement not found."));
    }
    
    [Test]
    public async Task UpdateAchievement_UpdatesCorrectAchievement()
    {
        var achievement1 = new Achievement ("Test", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1 };
        var achievement2 = new Achievement ("Test2", AchievementType.Exploration, 3, AchievementObjectiveType.Account) { Id = 2 };
        var achievements = new Achievement[] { achievement1, achievement2 };
        
        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();

        var expectedAchievement =
            new Achievement("TestUpdated", AchievementType.Exploration, 1, AchievementObjectiveType.Account) { Id = 1, Description = "TestUpdatedDescription"};
        var achievementToUpdate = new AchievementUpdateRequest(1, "TestUpdated", AchievementType.Exploration, 1, AchievementObjectiveType.Account, "TestUpdatedDescription");
        await _repository.UpdateAchievement(achievementToUpdate);
        
        var updatedAchievement = await _repository.GetAchievement(1);

        Assert.That(updatedAchievement, Is.EqualTo(expectedAchievement));
    }
}