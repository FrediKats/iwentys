using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Features.GithubIntegration.GithubIntegration;
using MediatR;

namespace Iwentys.Features.Gamification.Leaderboard
{
    public static class GetCodingRating
    {
        public class Query : IRequest<Response>
        {
            public Query(int? courseId, int skip, int take)
            {
                CourseId = courseId;
                Skip = skip;
                Take = take;
            }

            public int? CourseId { get; set; }
            public int Skip { get; }
            public int Take { get; }
        }

        public class Response
        {
            public Response(List<StudyLeaderboardRowDto> rating)
            {
                Rating = rating;
            }

            public List<StudyLeaderboardRowDto> Rating { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<StudyGroup> _studyGroupRepository;
            private readonly GithubIntegrationService _githubIntegrationService;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _githubIntegrationService = githubIntegrationService;
                _studyGroupRepository = unitOfWork.GetRepository<StudyGroup>();
            }

            protected override Response Handle(Query request)
            {
                List<StudyLeaderboardRowDto> result = _studyGroupRepository.Get()
                    .WhereIf(request.CourseId, q => q.StudyCourseId == request.CourseId)
                    .SelectMany(g => g.Students)
                    .AsEnumerable()
                    .Select(s => new StudyLeaderboardRowDto(s, _githubIntegrationService.User.GetGithubUser(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                    .OrderBy(a => a.Activity)
                    .Skip(request.Skip)
                    .Take(request.Take)
                    .ToList();

                return new Response(result);
            }
        }
    }
}