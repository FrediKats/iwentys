using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.DataAccess;
using Iwentys.EntityManager.ApiClient;
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
            //TODO: optimization?
            IReadOnlyCollection<SubjectTeachersDto> subjectTeachers = await _entityManagerApiClient.Teachers.Client.GetAllAsync();

            var results = new List<SubjectAssignmentJournalItemDto>();
            foreach (SubjectTeachersDto subjectTeachersDto in subjectTeachers)
            {
                List<SubjectAssignmentDto> assignmentDtos = await _context
                    .SubjectAssignments
                    .Where(sa => sa.SubjectId == subjectTeachersDto.SubjectId)
                    .ProjectTo<SubjectAssignmentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                results.Add(new SubjectAssignmentJournalItemDto
                {
                    Id = subjectTeachersDto.SubjectId,
                    Title = subjectTeachersDto.Name,
                    Assignments = assignmentDtos
                });
            }
            return new Response(results);
        }
    }
}