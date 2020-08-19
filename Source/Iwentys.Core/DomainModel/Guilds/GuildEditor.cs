using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.DomainModel.Guilds
{
    public class GuildEditor
    {
        public Student Student { get;}

        public GuildEditor(Student student)
        {
            Student = student;
        }
    }

    public static class GuildEditorExtensions
    {
        public static GuildEditor EnsureIsGuildEditor(this Student student, Guild guild)
        {
            GuildMember member = guild.Members.Find(m => m.MemberId == student.Id);

            if (member is null)
                throw InnerLogicException.Guild.IsNotGuildMember(student.Id);

            if (!member.MemberType.IsEditor())
                throw InnerLogicException.Guild.IsNotGuildEditor(student.Id);

            return new GuildEditor(student);
        }
    }
}