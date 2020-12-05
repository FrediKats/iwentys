using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Domain
{
    public class GuildEditor
    {
        public StudentEntity Student { get; }

        public GuildEditor(StudentEntity student)
        {
            Student = student;
        }
    }

    public static class GuildEditorExtensions
    {
        public static GuildEditor EnsureIsGuildEditor(this StudentEntity student, GuildEntity guild)
        {
            GuildMemberEntity member = guild.Members.Find(m => m.MemberId == student.Id);

            if (member is null)
                throw InnerLogicException.Guild.IsNotGuildMember(student.Id, guild.Id);

            if (!member.MemberType.IsEditor())
                throw InnerLogicException.Guild.IsNotGuildEditor(student.Id);

            return new GuildEditor(student);
        }
    }
}