using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class GuildRecruitmentRepository : IGenericRepository<GuildRecruitmentEntity, int>
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

        public Task<GuildRecruitmentEntity> ReadByIdAsync(int key)
        {
            return Read().FirstOrDefaultAsync(g => g.Id == key);
        }

        public async Task<GuildRecruitmentEntity> UpdateAsync(GuildRecruitmentEntity entity)
        {
            EntityEntry<GuildRecruitmentEntity> result = _dbContext.GuildRecruitment.Update(entity);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public Task<int> DeleteAsync(int key)
        {
            return _dbContext.GuildRecruitment.Where(gr => gr.Id == key).DeleteFromQueryAsync();
        }
    }
}