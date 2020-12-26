using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Guilds.Repositories
{
    public static class GuildRepositoryExtensions
    {
        public static GuildEntity ReadForStudent(this IGenericRepository<GuildMemberEntity> repository, int studentId)
        {
            return repository
                .Get()
                .Where(gm => gm.MemberId == studentId)
                .Where(GuildMemberEntity.IsMember())
                .Include(gm => gm.Guild.Members)
                .Include(gm => gm.Guild.PinnedProjects)
                .Select(gm => gm.Guild)
                .SingleOrDefault();
        }

        public static bool IsStudentHaveRequest(this IGenericRepository<GuildMemberEntity> repository, int studentId)
        {
            return repository
                .Get()
                .Where(m => m.Member.Id == studentId)
                .Any(m => m.MemberType == GuildMemberType.Requested);
        }
    }
}