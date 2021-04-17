using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Gamification;
using MediatR;

namespace Iwentys.Features.Gamification.StudyLeaderboard
{
    public class FindStudentLeaderboardPosition
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
            public Response(CourseLeaderboardRow position)
            {
                Position = position;
            }

            public CourseLeaderboardRow Position { get; set; }
        }

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<CourseLeaderboardRow> _courseLeaderboardRowRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _courseLeaderboardRowRepository = unitOfWork.GetRepository<CourseLeaderboardRow>();
            }

            protected override Response Handle(Query request)
            {
                CourseLeaderboardRow result = _courseLeaderboardRowRepository
                    .Get()
                    .FirstOrDefault(clr => clr.StudentId == request.StudentId);

                return new Response(result);
            }
        }
    }
}