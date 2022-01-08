using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.SubjectAssignments;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.SubjectAssignments
{
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

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                SubjectAssignmentSubmit subjectAssignmentSubmit = await _context.SubjectAssignmentSubmits.GetById(request.Arguments.SubjectAssignmentSubmitId);
                IwentysUser iwentysUser = await _context.IwentysUsers.GetById(request.AuthorizedUser.Id);

                subjectAssignmentSubmit.AddFeedback(iwentysUser, request.Arguments);

                _context.SubjectAssignmentSubmits.Update(subjectAssignmentSubmit);

                return new Response();
            }
        }
    }
}