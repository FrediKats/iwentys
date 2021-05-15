using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.Domain.SubjectAssignments.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public static class CreateSubjectAssignment
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser authorizedUser, SubjectAssignmentCreateArguments arguments)
            {
                Arguments = arguments;
                AuthorizedUser = authorizedUser;
            }

            public SubjectAssignmentCreateArguments Arguments { get; set; }
            public AuthorizedUser AuthorizedUser { get; set; }
        }

        public class Response
        {
            public Response(SubjectAssignmentDto subjectAssignment)
            {
                SubjectAssignment = subjectAssignment;
            }

            public SubjectAssignmentDto SubjectAssignment { get; set; }
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
                Subject subject = await _context.Subjects.GetById(request.Arguments.SubjectId);
                IwentysUser creator = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                var subjectAssignment = SubjectAssignment.Create(creator, subject, request.Arguments);

                _context.SubjectAssignments.Add(subjectAssignment);

                SubjectAssignmentDto result = await _context
                    .SubjectAssignments
                    .Where(sa => sa.Id == subjectAssignment.Id)
                    .Select(SubjectAssignmentDto.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}