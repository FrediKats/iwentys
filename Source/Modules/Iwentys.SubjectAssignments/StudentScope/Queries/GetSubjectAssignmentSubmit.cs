using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.SubjectAssignments
{
    public class GetSubjectAssignmentSubmit
    {
        public class Query : IRequest<Response>
        {
            public Query(int subjectAssignmentSubmitId, AuthorizedUser authorizedUser)
            {
                SubjectAssignmentSubmitId = subjectAssignmentSubmitId;
                AuthorizedUser = authorizedUser;
            }

            public int SubjectAssignmentSubmitId { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
        }

        public class Response
        {
            public Response(SubjectAssignmentSubmitDto submit)
            {
                Submit = submit;
            }

            public SubjectAssignmentSubmitDto Submit { get; set; }

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
                SubjectAssignmentSubmitDto result = await _context
                    .SubjectAssignmentSubmits
                    .Where(sas => sas.Id == request.SubjectAssignmentSubmitId)
                    .Select(sas => new SubjectAssignmentSubmitDto(sas))
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}