using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Study
{
    public class GetStudyGroupByCourseId
    {
        public class Query : IRequest<Response>
        {
            public int? CourseId { get; set; }

            public Query(int? courseId)
            {
                CourseId = courseId;
            }
        }

        public class Response
        {
            public Response(List<GroupProfileResponseDto> groups)
            {
                Groups = groups;
            }

            public List<GroupProfileResponseDto> Groups { get; set; }
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
                List<GroupProfileResponseDto> result = await _context
                    .StudyGroups
                    .WhereIf(request.CourseId, gs => gs.StudyCourseId == request.CourseId)
                    .Select(GroupProfileResponseDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}