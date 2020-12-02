using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.ViewModels;

namespace Iwentys.Features.Newsfeeds.Repositories
{
    public interface INewsfeedRepository
    {
        Task<SubjectNewsfeedEntity> CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, int authorId, int subjectId);
        Task<List<SubjectNewsfeedEntity>> ReadSubjectNewsfeeds(int subjectId);

        Task<List<GuildNewsfeedEntity>> GetGuildNewsfeeds(int guildId);
    }
}