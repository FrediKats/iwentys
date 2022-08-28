using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Assignments;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Assignments;

public static class UndoAssignmentComplete
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

            StudentAssignment studentAssignment = assignment.MarkUncompleted(student);

            _context.StudentAssignments.Update(studentAssignment);

            return new Response();
        }
    }
}