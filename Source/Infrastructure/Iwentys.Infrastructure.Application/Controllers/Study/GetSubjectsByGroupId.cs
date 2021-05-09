using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Repositories;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class GetSubjectsByGroupId
    {
        public class Query : IRequest<Response>
        {
            public int GroupId { get; set; }

            public Query(int groupId)
            {
                GroupId = groupId;
            }
        }

        public class Response
        {
            public Response(List<SubjectProfileDto> subjects)
            {
                Subjects = subjects;
            }

            public List<SubjectProfileDto> Subjects { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IwentysDbContext _context;

            public Handler(IwentysDbContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<SubjectProfileDto> result = await _context
                    .GroupSubjects
                    .SearchSubjects(StudySearchParametersDto.ForGroup(request.GroupId))
                    .Select(entity => new SubjectProfileDto(entity))
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}