using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.SubjectAssignments
{
    public class GetSubjectAssignmentForGroup
    {
        public class Query : IRequest<Response>
        {
            public Query(int groupId)
            {
                GroupId = groupId;
            }

            public int GroupId { get; set; }
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

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            //TODO: if user is not teacher - filter submits
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<SubjectAssignmentDto> result = await _context.GroupSubjectAssignments
                    .Where(gsa => gsa.GroupId == request.GroupId)
                    .Select(gsa => gsa.SubjectAssignment)
                    .Distinct()
                    .Select(SubjectAssignmentDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}