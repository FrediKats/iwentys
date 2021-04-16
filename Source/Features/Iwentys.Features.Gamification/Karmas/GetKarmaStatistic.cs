using System.Collections.Generic;
using System.Linq;
using Iwentys.Common.Databases;
using Iwentys.Domain.Gamification;
using MediatR;

namespace Iwentys.Features.Gamification.Karmas
{
    public class GetKarmaStatistic
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
            private readonly IGenericRepository<KarmaUpVote> _karmaRepository;

            public Handler(IUnitOfWork unitOfWork)
            {
                _karmaRepository = unitOfWork.GetRepository<KarmaUpVote>();
            }

            protected override Response Handle(Query request)
            {
                List<KarmaUpVote> karmaUpVotes = _karmaRepository
                    .Get()
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