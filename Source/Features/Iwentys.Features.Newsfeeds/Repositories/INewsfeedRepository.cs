using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Entities;

namespace Iwentys.Features.Newsfeeds.Repositories
{
    public interface INewsfeedRepository
    {
        Task<List<SubjectNewsfeedEntity>> GetSubjectNewsfeeds(int subjectId);
        Task<List<GuildNewsfeedEntity>> GetGuildNewsfeeds(int guildId);
    }
}