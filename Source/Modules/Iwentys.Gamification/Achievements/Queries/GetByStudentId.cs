using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Achievements;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Gamification
{
    public class GetByStudentId
    {
        public class Query : IRequest<Response>
        {
            public Query(int studentId)
            {
                StudentId = studentId;
            }

            public int StudentId { get; set; }
        }

        public class Response
        {
            public Response(List<AchievementInfoDto> achievements)
            {
                Achievements = achievements;
            }

            public List<AchievementInfoDto> Achievements { get; set; }
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
                List<AchievementInfoDto> result = _context
                    .StudentAchievements
                    .Where(a => a.StudentId == request.StudentId)
                    .Select(AchievementInfoDto.FromStudentsAchievement)
                    .ToList();

                return new Response(result);
            }
        }
    }
}
