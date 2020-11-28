using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Newsfeeds.Repositories;
using Iwentys.Models.Entities.Newsfeeds;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly INewsfeedRepository _newsfeedRepository;

        public NewsfeedService(INewsfeedRepository newsfeedRepository)
        {
            _newsfeedRepository = newsfeedRepository;
        }

        public Task<List<SubjectNewsfeedEntity>> GetSubjectNewsfeeds(int subjectId)
        {
            return _newsfeedRepository.GetSubjectNewsfeeds(subjectId);
        }
    }
}