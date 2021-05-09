using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public class SendSubmit
    {
        public class Query : IRequest<Response>
        {
            public Query(SubjectAssignmentSubmitCreateArguments arguments, AuthorizedUser authorizedUser)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }

            public SubjectAssignmentSubmitCreateArguments Arguments { get; set; }
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
                SubjectAssignment subjectAssignment = await _context.SubjectAssignments.GetById(request.Arguments.SubjectAssignmentId);

                SubjectAssignmentSubmit subjectAssignmentSubmit = subjectAssignment.CreateSubmit(request.AuthorizedUser, request.Arguments);

                _context.SubjectAssignmentSubmits.Add(subjectAssignmentSubmit);
                
                SubjectAssignmentSubmitDto result = await _context
                    .SubjectAssignmentSubmits
                    .Where(sas => sas.Id == subjectAssignmentSubmit.Id)
                    .Select(sas => new SubjectAssignmentSubmitDto(sas))
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}