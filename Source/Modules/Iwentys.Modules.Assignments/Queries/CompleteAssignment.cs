using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Modules.Assignments.Queries
{
    public static class CompleteAssignment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; }
            public int AssignmentId { get; }

            public Query(AuthorizedUser user, int assignmentId)
            {
                User = user;
                AssignmentId = assignmentId;
            }
        }

        public class Response
        {
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
                Student student = await _context.Students.GetById(request.User.Id);
                Assignment assignment = await _context.Assignments.GetById(request.AssignmentId);

                StudentAssignment studentAssignment = assignment.MarkCompleted(student);

                _context.StudentAssignments.Update(studentAssignment);
                return new Response();
            }
        }
    }
}