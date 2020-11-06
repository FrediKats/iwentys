using System.Linq;
using Iwentys.Models.Entities.Gamification;

namespace Iwentys.Features.Achievements
{
    public interface IAchievementRepository
    {
        StudentAchievementEntity CreateStudentAchievement(StudentAchievementEntity studentAchievement);
        IQueryable<StudentAchievementEntity> ReadStudentAchievements();
    }
}