using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Karmas;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Karmas
{
    public static class GetKarmaStatistic
    {
        public class Query : IRequest<Response>
        {
            public int StudentId { get; set; }

            public Query(int studentId)
            {
                StudentId = studentId;
            }
        }

        public class Response
        {
            public int StudentId { get; set; }
            public int Karma { get; set; }

            public List<int> UpVotes { get; set; }
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
                List<KarmaUpVote> karmaUpVotes = _context
                    .KarmaUpVotes
                    .Where(karma => karma.TargetId == request.StudentId)
                    .ToList();

                return new Response
                {
                    StudentId = request.StudentId,
                    Karma = karmaUpVotes.Count,
                    UpVotes = karmaUpVotes.Select(u => u.AuthorId).ToList()
                };
            }
        }
    }
}