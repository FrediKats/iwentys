using System;
using System.Linq;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildMemberEntity
    {
        protected GuildMemberEntity()
        {
        }

        public GuildMemberEntity(GuildEntity guild, StudentEntity student, GuildMemberType memberType)
            : this(guild.Id, student.Id, memberType)
        {
        }

        public GuildMemberEntity(int guildId, int studentId, GuildMemberType memberType) : this()
        {
            GuildId = guildId;
            MemberId = studentId;
            MemberType = memberType;
        }

        public int GuildId { get; set; }
        public virtual GuildEntity Guild { get; set; }

        public int MemberId { get; set; }
        public virtual StudentEntity Member { get; set; }

        public GuildMemberType MemberType { get; set; }
        
        public void MarkBlocked()
        {
            Member.GuildLeftTime = DateTime.UtcNow;
            MemberType = GuildMemberType.Blocked;
        }
    }
    
    public static class GuildMemberEntityExtensions
    {
        public static IQueryable<GuildMemberEntity> WhereIsMember(this IQueryable<GuildMemberEntity> queryable, int studentId)
        {
            return queryable
                .Where(gm => gm.MemberId == studentId)
                .Where(gm => gm.MemberType == GuildMemberType.Creator
                             || gm.MemberType == GuildMemberType.Mentor
                             || gm.MemberType == GuildMemberType.Member);
        }
    }
}