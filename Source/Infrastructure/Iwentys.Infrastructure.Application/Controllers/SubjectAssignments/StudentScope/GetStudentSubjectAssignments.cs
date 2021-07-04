using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.Infrastructure.Application.Controllers.SubjectAssignments.Dtos;
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
                var currentStudent = await _context.Students.GetById(request.User.Id);

                List<SubjectAssignmentDto> subjectAssignmentDtos = await _context
                    .GroupSubjectAssignments
                    .Where(gsa => gsa.GroupId == currentStudent.GroupId)
                    .Select(gsa => gsa.SubjectAssignment)
                    .Where(sa => sa.AvailableForStudent)
                    .ProjectTo<SubjectAssignmentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return new Response(subjectAssignmentDtos);
            }
        }
    }
}