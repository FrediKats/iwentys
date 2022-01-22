using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.WebService.Application;
using MediatR;
using StudySemester = Iwentys.Domain.Study.StudySemester;

namespace Iwentys.Gamification;

public class GetStudyRating
{
    public class Query : IRequest<Response>
    {
        public Query(int? groupId, int? subjectId, int? courseId, StudySemester? studySemester, int skip, int take)
        {
            GroupId = groupId;
            SubjectId = subjectId;
            CourseId = courseId;
            StudySemester = studySemester;
            Skip = skip;
            Take = take;
        }

        public int? GroupId { get; init; }
        public int? SubjectId { get; init; }
        public int? CourseId { get; init; }
        public StudySemester? StudySemester { get; init; }
        public int Skip { get; init; }
        public int Take { get; init; }
    }

    public class Response
    {
        public Response(List<StudyLeaderboardRowDto> leaders)
        {
            Leaders = leaders;
        }

        public List<StudyLeaderboardRowDto> Leaders { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, IwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            //TODO:refactor, remove this entity
            var searchParametersDto = new StudySearchParametersDto(request.GroupId, request.SubjectId, request.CourseId, request.StudySemester, request.Skip, request.Take);

            if (searchParametersDto.CourseId is null && searchParametersDto.GroupId is null)
                throw new IwentysExecutionException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");

            IReadOnlyCollection<SubjectActivity> result = await _context.GetStudentActivities(searchParametersDto);

            List<StudyLeaderboardRowDtoWithoutStudent> leaders = result
                .GroupBy(r => r.StudentId)
                .Select(g => new StudyLeaderboardRowDtoWithoutStudent(g.ToList()))
                .OrderByDescending(a => a.Activity)
                .Skip(searchParametersDto.Skip)
                .Take(searchParametersDto.Take)
                .ToList();

            IReadOnlyCollection<StudentInfoDto> studentInfoDtos = await _entityManagerApiClient.StudentProfiles.GetAsync();
            List<StudyLeaderboardRowDto> studyLeaderboardRowDtos = leaders
                .Join(studentInfoDtos,
                    l => l.StudentId,
                    s => s.Id,
                    (l, s) => new StudyLeaderboardRowDto(s, l.Activity))
                .ToList();

            return new Response(studyLeaderboardRowDtos);
        }
    }
}