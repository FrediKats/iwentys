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
    public class GetSubjectAssignmentForSubject
    {
        public class Query : IRequest<Response>
        {
            public Query(int subjectId)
            {
                SubjectId = subjectId;
            }

            public int SubjectId { get; set; }
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

            //TODO: it is not work?
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<SubjectAssignmentDto> result = await _context
                    .SubjectAssignments
                    .Where(sa => sa.SubjectId == request.SubjectId)
                    .Select(SubjectAssignmentDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}