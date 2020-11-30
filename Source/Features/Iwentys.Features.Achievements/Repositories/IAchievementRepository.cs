using System.Linq;
using Iwentys.Features.Achievements.Entities;

namespace Iwentys.Features.Achievements.Repositories
{
    public interface IAchievementRepository
    {
        StudentAchievementEntity CreateStudentAchievement(StudentAchievementEntity studentAchievement);
        IQueryable<StudentAchievementEntity> ReadStudentAchievements();
    }
}