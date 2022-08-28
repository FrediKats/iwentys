using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Assignments;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Student student = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(request.User.Id);
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