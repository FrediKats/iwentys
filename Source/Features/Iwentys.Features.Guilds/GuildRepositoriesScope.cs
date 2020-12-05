using Iwentys.Features.Guilds.Repositories;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Guilds
{
    public class GuildRepositoriesScope
    {
        public IStudentRepository Student;
        public IGuildRepository Guild;
        public IGuildMemberRepository GuildMember;
        public IGuildTributeRepository GuildTribute;

        public GuildRepositoriesScope(IStudentRepository student, IGuildRepository guild, IGuildMemberRepository guildMember, IGuildTributeRepository guildTribute)
        {
            Student = student;
            Guild = guild;
            GuildMember = guildMember;
            GuildTribute = guildTribute;
        }
    }
}