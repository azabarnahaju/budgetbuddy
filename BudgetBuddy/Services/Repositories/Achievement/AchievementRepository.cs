namespace BudgetBuddy.Services.Repositories.Achievement;
using BudgetBuddy.Model;

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

    public void AddAchievement(IEnumerable<Achievement> achievements)
    {
        _achievements.AddRange(achievements);
    }

    public void AddAchievement(Achievement achievement)
    {
        _achievements.Add(achievement);
    }

    public void DeleteAchievement(int id)
    {
        if (_achievements.Count == 0 || _achievements.All(a => a.Id != id)) throw new Exception("Achievement is not found.");

        _achievements = _achievements.Where(a => a.Id != id).ToList();
    }
    
    public void UpdateAchievement(Achievement achievement)
    {
        if (_achievements.FirstOrDefault(a => a.Id == achievement.Id) is null) throw new Exception("Achievement not found.");

        _achievements = _achievements.Select(a => a.Id == achievement.Id ? achievement : a).ToList();
    }
}