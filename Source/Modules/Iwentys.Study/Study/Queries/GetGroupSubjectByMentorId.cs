using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Study
{
    public class GetGroupSubjectByMentorId
    {
        public class Query : IRequest<Response>
        {
            public int? MentorId { get; set; }

            public Query(int? mentorId)
            {
                MentorId = mentorId;
            }
        }

        public class Response
        {
            public Response(List<GroupSubjectInfoDto> groups)
            {
                Groups = groups;
            }

            public List<GroupSubjectInfoDto> Groups { get; set; }
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
                List<GroupSubjectInfoDto> result = await _context
                    .GroupSubjects
                    .WhereIf(request.MentorId, gs => gs.Mentors.Any(m => m.UserId == request.MentorId))
                    .Select(GroupSubjectInfoDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}