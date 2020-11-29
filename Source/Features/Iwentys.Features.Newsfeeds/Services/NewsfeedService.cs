using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Newsfeeds.Repositories;
using Iwentys.Models.Entities.Newsfeeds;
using Iwentys.Models.Transferable.Study;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly INewsfeedRepository _newsfeedRepository;

        public NewsfeedService(INewsfeedRepository newsfeedRepository)
        {
            _newsfeedRepository = newsfeedRepository;
        }

        public async Task<List<NewsfeedInfoResponse>> GetSubjectNewsfeedsAsync(int subjectId)
        {
            List<SubjectNewsfeedEntity> result = await _newsfeedRepository.GetSubjectNewsfeeds(subjectId);
            return result.SelectToList(n => NewsfeedInfoResponse.Wrap(n.Newsfeed));
        }

        public async Task<List<NewsfeedInfoResponse>> GetGuildNewsfeeds(int guildId)
        {
            List<GuildNewsfeedEntity> result = await _newsfeedRepository.GetGuildNewsfeeds(guildId);
            return result.SelectToList(n => NewsfeedInfoResponse.Wrap(n.Newsfeed));
        }
    }
}