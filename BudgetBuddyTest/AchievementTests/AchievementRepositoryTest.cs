using BudgetBuddy.Data;
using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Achievement;
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
        await _context.AddAsync(new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() });
        await _context.SaveChangesAsync();
        
        var expectedResult = new Achievement[] { new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() } };
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
            new Achievement { Id = 1, Name = "b", Description = "bca", Users = new List<User>() },
            new Achievement { Id = 2, Name = "c", Description = "cba", Users = new List<User>() },
            new Achievement { Id = 3, Name = "d", Description = "ddd", Users = new List<User>() }
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
        var achievement = new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() };
        await _context.AddAsync(achievement);
        await _context.SaveChangesAsync();
        
        var actualResult = await _repository.GetAchievement(1);
        
        Assert.That(actualResult, Is.EqualTo(achievement));
    }
    
    [Test]
    public async Task AddAchievement_ThrowExceptionWhenAchievementAlreadyStored()
    {
        var achievement = new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() };
        await _context.AddAsync(achievement);
        await _context.SaveChangesAsync();

        var achievementsToAdd = new List<Achievement>
        {
            new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() }
        };
        
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.AddAchievement(achievementsToAdd));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"Achievement with ID {achievement.Id} already exists."));
    }
    
    [Test]
    public void AddAchievement_ThrowExceptionWhenAddingDuplicateAchievements()
    {
        var achievementsToAdd = new List<Achievement>
        {
            new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() },
            new Achievement { Id = 1, Name = "a", Description = "abc", Users = new List<User>() }
        };
        
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.AddAchievement(achievementsToAdd));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"You're trying to add duplicate achievements."));
    }
    
    [Test]
    public async Task AddAchievement_ReturnsCorrectAchievementsWhenAddedOne()
    {
        var achievements = new List<Achievement>
        {
            new Achievement { Id = 1, Name = "b", Description = "bca", Users = new List<User>() },
            new Achievement { Id = 2, Name = "c", Description = "cba", Users = new List<User>() },
        };
        var achievementToAdd = new List<Achievement>
        {
            new Achievement { Id = 3, Name = "d", Description = "ddd", Users = new List<User>() }
        };
        
        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
        
        var actualResult = await _repository.AddAchievement(achievementToAdd);
        var currentRepository = await _repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(currentRepository.Count(), Is.EqualTo(achievements.Count + achievementToAdd.Count()));
            Assert.That(actualResult.Count(), Is.EqualTo(achievementToAdd.Count()));
            Assert.That(actualResult, Is.EqualTo(achievementToAdd));
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
            new Achievement { Id = 1, Name = "b", Description = "bca", Users = new List<User>() },
            new Achievement { Id = 2, Name = "c", Description = "cba", Users = new List<User>() },
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
            new Achievement { Id = 1, Name = "b", Description = "bca", Users = new List<User>() },
            new Achievement { Id = 2, Name = "c", Description = "cba", Users = new List<User>() },
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
            Assert.That(repositoryAfter, Does.Not.Contain(new Achievement { Id = 1}));
        });
    }
    
    [Test]
    public async Task UpdateAchievement_ThrowsExceptionWhenAchievementNotExist()
    {
        var achievements = new List<Achievement>
        {
            new Achievement { Id = 1, Name = "b", Description = "bca", Users = new List<User>() },
            new Achievement { Id = 2, Name = "c", Description = "cba", Users = new List<User>() },
        };

        var achievementToUpdate = new Achievement { Id = 3, Name = "d", Description = "ddd", Users = new List<User>() };
        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
        
        var ex = Assert.ThrowsAsync<Exception>(() => _repository.UpdateAchievement(achievementToUpdate));
        Assert.That(ex, Is.InstanceOf<Exception>());
        Assert.That(ex.Message, Is.EqualTo($"Achievement not found."));
    }
    
    [Test]
    public async Task UpdateAchievement_UpdatesCorrectAchievement()
    {
        var achievement1 = new Achievement { Id = 1, Name = "b", Description = "bca", Users = new List<User>() };
        var achievement2 = new Achievement { Id = 2, Name = "c", Description = "cba", Users = new List<User>() };
        var achievements = new Achievement[] { achievement1, achievement2 };
        
        await _context.AddRangeAsync(achievements);
        await _context.SaveChangesAsync();
        
        var achievementUpdate = new Achievement { Id = 1, Name = "asdasdasd", Description = "asdasdasd", Users = new List<User>() };
        await _repository.UpdateAchievement(achievementUpdate);
        
        var updatedAchievement = await _repository.GetAchievement(1);

        Assert.That(updatedAchievement, Is.EqualTo(achievementUpdate));
    }
}