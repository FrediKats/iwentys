using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types;

namespace Iwentys.Core.DomainModel.Guilds
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