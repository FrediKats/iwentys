using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Gamification;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Leaderboard
{
    public static class CourseRatingForceRefresh
    {
        public class Query : IRequest<Response>
        {
            public Query(int courseId)
            {
                CourseId = courseId;
            }

            public int CourseId { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<CourseLeaderboardRow> oldRows = await _context
                    .CourseLeaderboardRows
                    .Where(clr => clr.CourseId == request.CourseId)
                    .ToListAsync();

                List<SubjectActivity> result = _context
                    .GetStudentActivities(new StudySearchParametersDto { CourseId = request.CourseId })
                    .ToList();
                
                List<CourseLeaderboardRow> newRows = CourseLeaderboardRow.Create(request.CourseId, result, oldRows);

                _context.CourseLeaderboardRows.RemoveRange(oldRows);
                _context.CourseLeaderboardRows.AddRange(newRows);
                return new Response();
            }
        }
    }
}