using Iwentys.Database.Context;
using Iwentys.Models.Entities.Gamification;

namespace Iwentys.Core.Gamification
{
    public class AchievementProvider
    {
        private readonly DatabaseAccessor _databaseAccessor;

        public AchievementProvider(DatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public void Achieve(AchievementModel achievement, int studentId)
        {
            _databaseAccessor.Context.StudentAchievements.Add(new StudentAchievementModel {StudentId = studentId, AchievementId = achievement.Id});
            _databaseAccessor.Context.SaveChanges();
        }
    }
}