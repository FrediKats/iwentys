using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class MakeGroupAdmin
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser Initiator { get; set; }
            public int NewGroupAdminId { get; set; }

            public Query(AuthorizedUser initiator, int newGroupAdminId)
            {
                Initiator = initiator;
                NewGroupAdminId = newGroupAdminId;
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
                IwentysUser initiatorProfile = await _context.IwentysUsers.GetById(request.Initiator.Id);
                Student newGroupAdminProfile = await _context.Students.GetById(request.NewGroupAdminId);

                StudyGroup studyGroup = StudyGroup.MakeGroupAdmin(initiatorProfile, newGroupAdminProfile);

                _context.StudyGroups.Update(studyGroup);

                return new Response();
            }
        }
    }
}