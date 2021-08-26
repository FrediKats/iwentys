using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Modules.AccountManagment.Dtos;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.AccountManagement.Mentors.Queries
{
    public class GetAllSubjectsMentors
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; set; }
            
            public Query(AuthorizedUser authorizedUser)
            {
                AuthorizedUser = authorizedUser;
            }
        }

        public class Response
        {
            public IReadOnlyList<SubjectMentorsDto> SubjectMentors { get; set; }

            public Response(IReadOnlyList<SubjectMentorsDto> subjectMentors)
            {
                SubjectMentors = subjectMentors;
            }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private IwentysDbContext _context;
            private IMapper _mapper;

            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var subjectMentors = await
                    _context.GroupSubjects.Include(g => g.LectorMentor)
                            .Include(g=>g.StudyGroup)
                            .Include(g => g.PracticeMentors)
                            .ThenInclude(p => p.User)
                            .ToListAsync(cancellationToken);

                var subjectMentorsDtos = _mapper.Map<List<SubjectMentorsDto>>(subjectMentors.GroupBy(x => x.SubjectId));
                
                return new Response(subjectMentorsDtos);
            }
        }
    }
}