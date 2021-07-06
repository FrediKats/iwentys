using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.Domain.Study;
using Iwentys.Domain.SubjectAssignments.Enums;
using Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos;
using Iwentys.Infrastructure.Application.Specifications.SubjectAssignments;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
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


            public Handler(IwentysDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Student currentStudent = await _context.Students.GetById(request.User.Id);

                List<SubjectAssignmentDto> subjectAssignmentDtos = await _context
                    .GroupSubjectAssignments
                    .Specify(new StudentSubjectAssignmentSpecification(currentStudent))
                    .ProjectTo<SubjectAssignmentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new Response(subjectAssignmentDtos);
            }
        }
    }
}