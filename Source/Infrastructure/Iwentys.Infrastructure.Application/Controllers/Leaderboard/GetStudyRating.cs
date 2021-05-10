using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Enums;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Subcontext;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Leaderboard
{
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

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                //TODO:refactor, remove this entity
                var searchParametersDto = new StudySearchParametersDto(request.GroupId, request.SubjectId, request.CourseId, request.StudySemester, request.Skip, request.Take);

                if (searchParametersDto.CourseId is null && searchParametersDto.GroupId is null)
                    throw new IwentysExecutionException("One of StudySearchParametersDto fields: CourseId or GroupId should be null");

                List<SubjectActivity> result = _context.GetStudentActivities(searchParametersDto).ToList();

                List<StudyLeaderboardRowDto> leaders = result
                    .GroupBy(r => r.StudentId)
                    .Select(g => new StudyLeaderboardRowDto(g.ToList()))
                    .OrderByDescending(a => a.Activity)
                    .Skip(searchParametersDto.Skip)
                    .Take(searchParametersDto.Take)
                    .ToList();

                return new Response(leaders);
            }
        }
    }
}