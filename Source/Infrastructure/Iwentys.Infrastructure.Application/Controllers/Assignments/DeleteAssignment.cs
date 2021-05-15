using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Assignments
{
    public static class DeleteAssignment
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

                if (student.Id != assignment.AuthorId)
                    throw InnerLogicException.AssignmentExceptions.IsNotAssignmentCreator(assignment.Id, student.Id);

                //FYI: it's coz for dropped cascade. Need to rework after adding cascade deleting
                List<StudentAssignment> studentAssignments = await _context.StudentAssignments
                    .Where(sa => sa.AssignmentId == request.AssignmentId)
                    .ToListAsync();

                _context.StudentAssignments.RemoveRange(studentAssignments);
                _context.Assignments.Remove(assignment);

                return new Response();
            }
        }
    }
}