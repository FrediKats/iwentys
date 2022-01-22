using System.Threading;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.Study;

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
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser initiatorProfile = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.Initiator.Id);
            Student newGroupAdminProfile = await _context.Students.GetById(request.NewGroupAdminId);

            StudyGroup studyGroup = StudyGroup.MakeGroupAdmin(initiatorProfile, newGroupAdminProfile);

            _context.StudyGroups.Update(studyGroup);

            return new Response();
        }
    }
}