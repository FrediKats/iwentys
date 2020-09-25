﻿using System;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Database.Repositories.Abstractions
{
    public interface IGuildMemberRepository
    {
        Boolean IsStudentHaveRequest(int studentId);

        GuildMemberEntity AddMember(GuildEntity guild, StudentEntity student, GuildMemberType memberType);
        GuildMemberEntity UpdateMember(GuildMemberEntity member);
        void RemoveMember(int guildId, int userId);
    }
}