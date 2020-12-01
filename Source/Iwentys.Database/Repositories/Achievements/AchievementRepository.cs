using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Features.Achievements.Entities;
using Iwentys.Features.Achievements.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Achievements
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly IwentysDbContext _dbContext;

        public AchievementRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentAchievementEntity CreateStudentAchievement(StudentAchievementEntity studentAchievement)
        {
            EntityEntry<StudentAchievementEntity> entity = _dbContext.StudentAchievements.Add(studentAchievement);
            _dbContext.SaveChanges();
            return entity.Entity;
        }

        public IQueryable<StudentAchievementEntity> ReadStudentAchievements()
        {
            return _dbContext.StudentAchievements.Include(sa => sa.Achievement);
        }
    }
}