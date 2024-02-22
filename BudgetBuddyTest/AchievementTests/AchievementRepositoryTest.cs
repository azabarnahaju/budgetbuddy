using BudgetBuddy.Model;
using BudgetBuddy.Services.Repositories.Achievement;
using Microsoft.Extensions.Logging;
using Moq;

namespace BudgetBuddyTest.AchievementTests;

[TestFixture]
public class AchievementRepositoryTest
{
    
    [Test]
    public void GetAllAchievements_ReturnsEmptyArrayWhenNoAchievements()
    {
        var repository = new AchievementRepository(new List<Achievement>());
        var expectedResult = Array.Empty<Achievement>();
        var actualResult = repository.GetAllAchievements();
        
        Assert.That(actualResult, Is.EquivalentTo(expectedResult));
    }

    [Test]
    public void GetAllAchievements_ReturnsOneWhenHasOneAchievement()
    {
        var repository = new AchievementRepository(new List<Achievement>() { new Achievement { Id = 1} });
        var expectedResult = new Achievement[] { new Achievement { Id = 1} };
        var actualResult = repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Count(), Is.EqualTo(1));
            Assert.That(actualResult, Is.EquivalentTo(expectedResult));
        });
    }
    
    [Test]
    public void GetAllAchievements_ReturnsMultipleWhenHasMultipleAchievement()
    {
        var achievements = new List<Achievement>() { new Achievement { Id = 1 }, new Achievement { Id = 2 } };
        var repository = new AchievementRepository(achievements);
        var expectedResult = new Achievement[] { new Achievement { Id = 1}, new Achievement { Id = 2} };
        var actualResult = repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(actualResult.Count, Is.EqualTo(achievements.Count));
            Assert.That(actualResult, Is.EquivalentTo(expectedResult));
        });
        
    }
    
    [Test]
    public void GetAchievement_ReturnsExceptionWhenIdIncorrect()
    {
        var repository = new AchievementRepository(new List<Achievement>());
        
        Assert.Throws<Exception>(() => repository.GetAchievement(1));
    }
    
    [Test]
    public void GetAchievement_ReturnsCorrectAchievementForId()
    {
        var repository = new AchievementRepository(new List<Achievement> { new Achievement { Id = 1 }});
        var expectedResult = new Achievement { Id = 1 };
        var actualResult = repository.GetAchievement(1);
        
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
    
    [Test]
    public void AddAchievement_ThrowExceptionWhenAchievementAlreadyStored()
    {
        var repository = new AchievementRepository(new List<Achievement> { new Achievement { Id = 1 }});
        
        Assert.Throws<Exception>(() => repository.AddAchievement(new List<Achievement> { new Achievement { Id = 1 } }));
    }
    
    [Test]
    public void AddAchievement_ThrowExceptionWhenAddingDuplicateAchievements()
    {
        var repository = new AchievementRepository(new List<Achievement>() );
        repository.AddAchievement(new List<Achievement> { new Achievement { Id = 1 } });
        
        Assert.Throws<Exception>(() => repository.AddAchievement(new List<Achievement> { new Achievement { Id = 1 } }));
    }
    
    [Test]
    public void AddAchievement_ReturnsCorrectAchievementsWhenAddedOne()
    {
        var achievements = new Achievement[] { new Achievement { Id = 1}, new Achievement { Id = 2}};
        var repository = new AchievementRepository(achievements.ToList());
        var achievementToAdd = new List<Achievement> { new Achievement { Id = 3 } };
        var actualResult = repository.AddAchievement(achievementToAdd);
        var currentRepository = repository.GetAllAchievements();

        Console.WriteLine(achievements.Length);
        Console.WriteLine(achievementToAdd.Count);
        
        Assert.Multiple(() =>
        {
            Assert.That(currentRepository.Count(), Is.EqualTo(achievements.Length + achievementToAdd.Count()));
            Assert.That(actualResult.Count(), Is.EqualTo(achievementToAdd.Count()));
            Assert.That(actualResult, Is.EqualTo(achievementToAdd));
        });
    }

    [Test]
    public void DeleteAchievement_ThrowsExceptionWhenAchievementRepositoryEmpty()
    {
        var repository = new AchievementRepository(new List<Achievement>() );

        Assert.Throws<Exception>(() => repository.DeleteAchievement(1));
    }

    [Test]
    public void DeleteAchievement_ThrowsExceptionWhenIdNotExist()
    {
        var achievements = new Achievement[] { new Achievement { Id = 1}, new Achievement { Id = 2}};
        var repository = new AchievementRepository(achievements.ToList());
        var currentRepository = repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.Throws<Exception>(() => repository.DeleteAchievement(3));
            Assert.That(achievements.Length, Is.EqualTo(currentRepository.Count()));
            Assert.That(achievements.ToList(), Is.EquivalentTo(currentRepository));
        });
    }

    [Test]
    public void DeleteAchievement_DeletesAchievementFromRepository()
    {
        var achievements = new Achievement[] { new Achievement { Id = 1}, new Achievement { Id = 2}};
        var repository = new AchievementRepository(achievements.ToList());
        repository.DeleteAchievement(1);
        var currentRepository = repository.GetAllAchievements();
        
        Assert.Multiple(() =>
        {
            Assert.That(currentRepository.Count(), Is.EqualTo(achievements.Count() - 1));
            Assert.That(currentRepository, Does.Not.Contain(new Achievement { Id = 1}));
        });
    }

    [Test]
    public void UpdateAchievement_ThrowsExceptionWhenAchievementNotExist()
    {
        var achievements = new Achievement[] { new Achievement { Id = 1 }, new Achievement { Id = 2 } };
        var repository = new AchievementRepository(achievements.ToList());
        
        Assert.Throws<Exception>(() => repository.UpdateAchievement(new Achievement { Id = 5 }));
    }
    
    [Test]
    public void UpdateAchievement_UpdatesCorrectAchievement()
    {
        var achievement1 = new Achievement { Id = 1 };
        var achievement2 = new Achievement { Id = 2 } ;
        var achievements = new Achievement[] { achievement1, achievement2 };
        
        var repository = new AchievementRepository(achievements.ToList());
        
        var achievementUpdate = new Achievement { Id = 1, Name = "abcd" };
        repository.UpdateAchievement(achievementUpdate);
        
        var updatedAchievement = repository.GetAchievement(1);
        
        Assert.Multiple(() =>
        {
            Assert.That(updatedAchievement, Does.Not.EqualTo(achievement1));
            Assert.That(updatedAchievement, Is.EqualTo(achievementUpdate));
        });
    }
}