namespace BudgetBuddy.Services.Repositories.Achievement;

using Model;

public class AchievementRepository : IAchievementRepository
{
    private List<Achievement> _achievements = new List<Achievement>();

    public IEnumerable<Achievement> GetAllAchievements()
    {
        return _achievements;
    }

    public Achievement GetAchievement(int id)
    {
        return _achievements.All(a => a.Id != id)
            ? throw new Exception("Achievement could not be found.")
            : _achievements.First(a => a.Id == id);
    }

    public IEnumerable<Achievement> AddAchievement(IEnumerable<Achievement> achievements)
    {
        foreach (var achievement in achievements)
        {
            if (_achievements.Any(a => a.Id == achievement.Id)) 
                throw new Exception($"Achievement with ID {achievement.Id} already exists.");
        }
        
        _achievements.AddRange(achievements);
        return achievements;
    }

    public void DeleteAchievement(int id)
    {
        if (_achievements.Count == 0 || _achievements.All(a => a.Id != id)) throw new Exception("Achievement is not found.");

        _achievements = _achievements.Where(a => a.Id != id).ToList();
    }
    
    public Achievement UpdateAchievement(Achievement achievement)
    {
        if (_achievements.FirstOrDefault(a => a.Id == achievement.Id) is null) throw new Exception("Achievement not found.");

        _achievements = _achievements.Select(a => a.Id == achievement.Id ? achievement : a).ToList();
        return _achievements.First(a => a.Id == achievement.Id);
    }
}