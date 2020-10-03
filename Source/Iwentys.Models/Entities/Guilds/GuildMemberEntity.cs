﻿using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Guilds
{
    public class GuildMemberEntity
    {
        public GuildMemberEntity()
        {
        }

        public GuildMemberEntity(GuildEntity guild, StudentEntity student, GuildMemberType memberType) : this(guild.Id, student.Id, memberType)
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
    }
}