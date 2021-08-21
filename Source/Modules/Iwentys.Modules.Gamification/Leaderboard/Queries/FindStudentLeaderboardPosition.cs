﻿using System.Linq;
using Iwentys.Domain.Gamification;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Leaderboard
{
    public static class FindStudentLeaderboardPosition
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
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            protected override Response Handle(Query request)
            {
                CourseLeaderboardRow result = _context
                    .CourseLeaderboardRows
                    .FirstOrDefault(clr => clr.StudentId == request.StudentId);

                return new Response(result);
            }
        }
    }
}