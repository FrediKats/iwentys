using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories.Implementations
{
    public class GuildRecruitmentRepository : IGuildRecruitmentRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildRecruitmentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GuildRecruitmentEntity Create(GuildEntity guild, GuildMemberEntity creator, string description)
        {
            EntityEntry<GuildRecruitmentEntity> recruitment = _dbContext.GuildRecruitment.Add(new GuildRecruitmentEntity
            {
                Description = description,
                GuildId = guild.Id
            });
            _dbContext.GuildRecruitmentMembers.Add(new GuildRecruitmentMemberEntity
            {
                GuildRecruitmentId = recruitment.Entity.Id,
                MemberId = creator.MemberId
            });

            _dbContext.SaveChanges();
            return recruitment.Entity;
        }

        public IQueryable<GuildRecruitmentEntity> Read()
        {
            return _dbContext.GuildRecruitment.Include(r => r.RecruitmentMembers);
        }

        public GuildRecruitmentEntity ReadById(int key)
        {
            return Read().FirstOrDefault(g => g.Id == key);
        }

        public GuildRecruitmentEntity Update(GuildRecruitmentEntity entity)
        {
            EntityEntry<GuildRecruitmentEntity> result = _dbContext.GuildRecruitment.Update(entity);
            _dbContext.SaveChanges();
            return result.Entity;
        }

        public void Delete(int key)
        {
            _dbContext.GuildRecruitment.Remove(ReadById(key));
            _dbContext.SaveChanges();
        }
    }
}