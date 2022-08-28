using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.SubjectAssignments;

public static class GetStudentSubjectAssignments
{
    public class Query : IRequest<Response>
    {
        public AuthorizedUser User { get; set; }
        public int SubjectId { get; set; }

        public Query(AuthorizedUser user, int subjectId)
        {
            User = user;
            SubjectId = subjectId;
        }
    }

    public class Response
    {
        public Response(List<SubjectAssignmentDto> subjectAssignments)
        {
            SubjectAssignments = subjectAssignments;
        }

        public List<SubjectAssignmentDto> SubjectAssignments { get; set; }
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
            Student currentStudent = await _entityManagerApiClient.StudentProfiles.GetByIdAsync(request.User.Id);

            List<SubjectAssignmentDto> subjectAssignmentDtos = await _context
                .GroupSubjectAssignments
                .Specify(new StudentSubjectAssignmentSpecification(currentStudent, request.SubjectId))
                .ProjectTo<SubjectAssignmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new Response(subjectAssignmentDtos);
        }
    }
}