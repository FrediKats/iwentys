using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.SubjectAssignments;

public static class SendSubmitFeedback
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser authorizedUser, SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            Arguments = arguments;
            AuthorizedUser = authorizedUser;
        }

        public SubjectAssignmentSubmitFeedbackArguments Arguments { get; set; }
        public AuthorizedUser AuthorizedUser { get; set; }
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
            SubjectAssignmentSubmit subjectAssignmentSubmit = await _context.SubjectAssignmentSubmits.GetById(request.Arguments.SubjectAssignmentSubmitId);
            IwentysUser iwentysUser = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            bool hasPermission = await _entityManagerApiClient.Teachers.HasTeacherPermissionAsync(request.AuthorizedUser.Id, subjectAssignmentSubmit.SubjectAssignment.SubjectId);
            if (!hasPermission)
                throw InnerLogicException.StudyExceptions.UserHasNotTeacherPermission(request.AuthorizedUser.Id);

            subjectAssignmentSubmit.AddFeedback(iwentysUser, request.Arguments);

            _context.SubjectAssignmentSubmits.Update(subjectAssignmentSubmit);

            return new Response();
        }
    }
}