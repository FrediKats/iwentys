using System.Linq;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Achievements;
using Iwentys.Domain.Gamification;

namespace Iwentys.WebService.Application;

public class AchievementHack
{

    public static async Task ProcessAchievement(AchievementProvider provider, IwentysDbContext context)
    {
        var achievementHack = new AchievementHack(context);
        foreach (GuildAchievement guildAchievement in provider.GuildAchievement)
        {
            achievementHack.AchieveForGuild(guildAchievement.AchievementId, guildAchievement.GuildId);
        }

        foreach (StudentAchievement achievement in provider.StudentAchievement)
        {
            achievementHack.Achieve(achievement.AchievementId, achievement.StudentId);
        }

        await context.SaveChangesAsync();
    }

    private readonly IwentysDbContext _context;

    public AchievementHack(IwentysDbContext context)
    {
        _context = context;
    }

    public void Achieve(int achievementId, int studentId)
    {
        if (_context.StudentAchievements.Any(s => s.AchievementId == achievementId && s.StudentId == studentId))
            return;

        _context.StudentAchievements.Add(StudentAchievement.Create(studentId, achievementId));
    }

    public void AchieveForGuild(int achievementId, int guildId)
    {
        if (_context.GuildAchievements.Any(s => s.AchievementId == achievementId && s.GuildId == guildId))
            return;

        _context.GuildAchievements.Add(GuildAchievement.Create(guildId, achievementId));
    }
}