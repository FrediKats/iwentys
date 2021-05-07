using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Assignments
{
    public static class CreateAssignment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; }
            public AssignmentCreateArguments AssignmentCreateArguments { get; }

            public Query(AuthorizedUser user, AssignmentCreateArguments assignmentCreateArguments)
            {
                User = user;
                AssignmentCreateArguments = assignmentCreateArguments;
            }
        }

        public class Response
        {
            public Response(List<AssignmentInfoDto> assignmentInfos)
            {
                AssignmentInfos = assignmentInfos;
            }

            public List<AssignmentInfoDto> AssignmentInfos { get; set; }
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
                Student author = await _context.Students.GetById(request.User.Id);

                List<StudentAssignment> assignments = StudentAssignment.Create(author, request.AssignmentCreateArguments);

                _context.StudentAssignments.AddRange(assignments);

                return new Response(assignments.Select(a => new AssignmentInfoDto(a)).ToList());
            }
        }
    }
}