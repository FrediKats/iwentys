using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Repositories
{
    public static class GuildRepositoryExtensions
    {
        //TODO: ensure that we do not call .Members after .SingleOrDefault
        public static Guild ReadForStudent(this IGenericRepository<GuildMember> repository, int studentId)
        {
            return repository
                .Get()
                .Where(gm => gm.MemberId == studentId)
                .Where(GuildMember.IsMember())
                .Include(gm => gm.Guild.Members)
                .Include(gm => gm.Guild.PinnedProjects)
                .Select(gm => gm.Guild)
                .SingleOrDefault();
        }

        public static bool IsStudentHaveRequest(this IGenericRepository<GuildMember> repository, int studentId)
        {
            return repository
                .Get()
                .Where(m => m.Member.Id == studentId)
                .Any(m => m.MemberType == GuildMemberType.Requested);
        }
    }
}