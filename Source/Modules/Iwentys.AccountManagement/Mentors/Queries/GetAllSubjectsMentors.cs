using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement
{
    public class GetAllSubjectsMentors
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser User { get; init; }
            
            public Query(AuthorizedUser authorizedUser)
            {
                User = authorizedUser;
            }
        }

        public class Response
        {
            public IReadOnlyList<SubjectMentorsDto> SubjectMentors { get; init; }

            public Response(IReadOnlyList<SubjectMentorsDto> subjectMentors)
            {
                SubjectMentors = subjectMentors;
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;
            private readonly IMapper _mapper;

            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser user = await _context.IwentysUsers.GetById(request.User.Id);
                
                var groupSubjects = await _context.GroupSubjects
                                                   .ToListAsync(cancellationToken);

                if (!user.IsAdmin && !groupSubjects.Any(gs => gs.HasMentorPermission(user)))
                {
                    throw InnerLogicException.StudyExceptions.UserIsNotMentor(user.Id);
                }

                var subjectMentorsDtos = _mapper.Map<List<SubjectMentorsDto>>(groupSubjects.GroupBy(x => x.SubjectId));
                
                return new Response(subjectMentorsDtos);
            }
        }
    }
}