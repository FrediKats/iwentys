using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;

namespace Iwentys.AccountManagement;

public class GetMentorsByGroupSubjectId
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; init; }
        public int GroupSubjectId { get; set; }
        public Query(AuthorizedUser user, int groupSubjectId)
        {
            User = user;
            GroupSubjectId = groupSubjectId;
        }
    }
        
    public class Response
    {
        public GroupMentorsDto GroupMentors { get; init; }

        public Response(GroupMentorsDto groupMentors)
        {
            GroupMentors = groupMentors;
        }
    }
        
    public class Handler : IRequestHandler<Query,Response>
    {
        private readonly IwentysDbContext _context;
        private readonly IMapper _mapper;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, IMapper mapper, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _mapper = mapper;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            IwentysUser user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.User.Id);
                
            if (!user.IsAdmin && !_context.GroupSubjects.Any(gs => gs.HasMentorPermission(user)))
            {
                throw InnerLogicException.StudyExceptions.UserIsNotMentor(user.Id);
            }

            var groupSubject = await _context.GroupSubjects.GetById(request.GroupSubjectId);
                
            var groupMentorsDtos = _mapper.Map<GroupMentorsDto>(groupSubject);

            return new Response(groupMentorsDtos);
        }
    }
}