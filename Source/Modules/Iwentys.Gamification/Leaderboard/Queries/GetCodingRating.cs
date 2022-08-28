using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Gamification;

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

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, GithubIntegrationService githubIntegrationService, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _githubIntegrationService = githubIntegrationService;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<Student> studentsFromCourse;
            if (request.CourseId is not null)
                studentsFromCourse = await _entityManagerApiClient.StudentProfiles.GetByCourseIdAsync(request.CourseId.Value);
            else
                studentsFromCourse = await _entityManagerApiClient.StudentProfiles.GetAsync();

            List<StudyLeaderboardRowDto> result = studentsFromCourse
                .Select(s => new StudyLeaderboardRowDto(s, _githubIntegrationService.User.GetGithubUser(s.GithubUsername).Result?.ContributionFullInfo.Total ?? 0))
                .OrderBy(a => a.Activity)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToList();

            return new Response(result);
        }
    }
}