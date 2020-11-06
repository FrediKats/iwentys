using System.Linq;
using System.Threading.Tasks;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Tools;
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

        public async Task<GuildRecruitmentEntity> CreateAsync(GuildEntity guild, GuildMemberEntity creator, string description)
        {
            EntityEntry<GuildRecruitmentEntity> recruitment = await _dbContext.GuildRecruitment.AddAsync(new GuildRecruitmentEntity
            {
                Description = description,
                GuildId = guild.Id
            });
            await _dbContext.GuildRecruitmentMembers.AddAsync(new GuildRecruitmentMemberEntity
            {
                GuildRecruitmentId = recruitment.Entity.Id,
                MemberId = creator.MemberId
            });

            await _dbContext.SaveChangesAsync();
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