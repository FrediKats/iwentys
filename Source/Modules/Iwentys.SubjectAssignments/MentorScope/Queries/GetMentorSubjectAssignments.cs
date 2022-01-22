using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.SubjectAssignments;

public class GetMentorSubjectAssignments
{
    public class Query : IRequest<Response>
    {
        public Query(AuthorizedUser user)
        {
            User = user;
        }

        public AuthorizedUser User { get; set; }
    }

    public class Response
    {
        public Response(List<SubjectAssignmentJournalItemDto> subjectAssignments)
        {
            SubjectAssignments = subjectAssignments;
        }

        public List<SubjectAssignmentJournalItemDto> SubjectAssignments { get; set; }
    }

    public class Handler : IRequestHandler<Query, Response>
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

            if (user.IsAdmin)
            {
                List<SubjectAssignmentJournalItemDto> assignments = await _context
                    .Subjects
                    .ProjectTo<SubjectAssignmentJournalItemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new Response(assignments);
            }
            else
            {
                List<SubjectAssignmentJournalItemDto> assignments = await _context
                    .Subjects
                    .Where(Subject.IsAllowedFor(user.Id))
                    .ProjectTo<SubjectAssignmentJournalItemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new Response(assignments);
            }
        }
    }
}