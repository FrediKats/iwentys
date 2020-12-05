using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Newsfeeds.Repositories;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly INewsfeedRepository _newsfeedRepository;

        public NewsfeedService(INewsfeedRepository newsfeedRepository)
        {
            _newsfeedRepository = newsfeedRepository;
        }

        public async Task<NewsfeedViewModel> CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, int authorId, int subjectId)
        {
            //TODO: add validation
            SubjectNewsfeedEntity subjectNewsfeedEntity = await _newsfeedRepository.CreateSubjectNewsfeed(createViewModel, authorId, subjectId);

            return NewsfeedViewModel.Wrap(subjectNewsfeedEntity.Newsfeed);
        }

        public async Task<List<NewsfeedViewModel>> GetSubjectNewsfeedsAsync(int subjectId)
        {
            List<SubjectNewsfeedEntity> result = await _newsfeedRepository.ReadSubjectNewsfeeds(subjectId);
            return result.SelectToList(n => NewsfeedViewModel.Wrap(n.Newsfeed));
        }

        public async Task<List<NewsfeedViewModel>> GetGuildNewsfeeds(int guildId)
        {
            List<GuildNewsfeedEntity> result = await _newsfeedRepository.GetGuildNewsfeeds(guildId);
            return result.SelectToList(n => NewsfeedViewModel.Wrap(n.Newsfeed));
        }
    }
}