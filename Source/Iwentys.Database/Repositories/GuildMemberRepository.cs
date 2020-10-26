using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
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

        public GuildMemberEntity AddMember(GuildEntity guild, StudentEntity student, GuildMemberType memberType)
        {
            EntityEntry<GuildMemberEntity> addedEntity = _dbContext.GuildMembers.Add(new GuildMemberEntity(guild, student, memberType));
            _dbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public GuildMemberEntity UpdateMember(GuildMemberEntity member)
        {
            EntityEntry<GuildMemberEntity> updatedEntity = _dbContext.GuildMembers.Update(member);

            _dbContext.SaveChanges();

            return updatedEntity.Entity;
        }

        public void RemoveMember(int guildId, int userId)
        {
            GuildMemberEntity guildMember = _dbContext.GuildMembers.Single(gm => gm.GuildId == guildId && gm.MemberId == userId);
            if (guildMember.MemberType == GuildMemberType.Creator)
                throw InnerLogicException.Guild.CreatorCannotLeave(userId, guildId);
            _dbContext.GuildMembers.Remove(guildMember);

            _dbContext.SaveChanges();
        }
    }
}