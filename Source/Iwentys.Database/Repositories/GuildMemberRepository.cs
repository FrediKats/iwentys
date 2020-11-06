using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class GuildMemberRepository
    {
        private readonly IwentysDbContext _dbContext;

        public GuildMemberRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsStudentHaveRequest(int studentId)
        {
            return !_dbContext.GuildMembers
                .Where(m => m.Member.Id == studentId)
                .Any(m => m.MemberType == GuildMemberType.Requested);
        }

        public async Task<GuildMemberEntity> AddMemberAsync(GuildEntity guild, StudentEntity student, GuildMemberType memberType)
        {
            EntityEntry<GuildMemberEntity> addedEntity = await _dbContext.GuildMembers.AddAsync(new GuildMemberEntity(guild, student, memberType));
            await _dbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<GuildMemberEntity> UpdateMemberAsync(GuildMemberEntity member)
        {
            EntityEntry<GuildMemberEntity> updatedEntity = _dbContext.GuildMembers.Update(member);

            await _dbContext.SaveChangesAsync();

            return updatedEntity.Entity;
        }

        public void RemoveMemberAsync(int guildId, int userId)
        {
            GuildMemberEntity guildMember = _dbContext.GuildMembers.Single(gm => gm.GuildId == guildId && gm.MemberId == userId);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.CreatorCannotLeave(userId, guildId);
            _dbContext.GuildMembers.Remove(guildMember);

            _dbContext.SaveChanges();
        }
    }
}