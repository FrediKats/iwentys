using System;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Entities
{
    public class GuildMemberEntity
    {
        private GuildMemberEntity()
        {
        }

        public GuildMemberEntity(GuildEntity guild, StudentEntity student, GuildMemberType memberType)
            : this(guild.Id, student.Id, memberType)
        {
        }

        public GuildMemberEntity(int guildId, int studentId, GuildMemberType memberType)
        {
            GuildId = guildId;
            MemberId = studentId;
            MemberType = memberType;
        }

        public int GuildId { get; set; }
        public GuildEntity Guild { get; set; }

        public int MemberId { get; set; }
        public StudentEntity Member { get; set; }

        public GuildMemberType MemberType { get; set; }
        
        public void MarkBlocked()
        {
            Member.GuildLeftTime = DateTime.UtcNow;
            MemberType = GuildMemberType.Blocked;
        }
    }
}