using Iwentys.Common.Exceptions;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Domain
{
    //TODO: delete, replace with guild mentor
    public class GuildEditor
    {
        public Student Student { get; }

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
                throw InnerLogicException.GuildExceptions.IsNotGuildMember(student.Id, guild.Id);

            if (!member.MemberType.IsEditor())
                throw InnerLogicException.GuildExceptions.IsNotGuildEditor(student.Id);

            return new GuildEditor(student);
        }

        public static GuildEditor EnsureIsGuildEditor(this Student student, GuildMember member)
        {
            if (!member.MemberType.IsEditor())
                throw InnerLogicException.GuildExceptions.IsNotGuildEditor(student.Id);

            return new GuildEditor(student);
        }
    }
}