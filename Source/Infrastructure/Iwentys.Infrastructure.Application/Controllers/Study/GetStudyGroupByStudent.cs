using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Controllers.Study.Dtos;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class GetStudyGroupByStudent
    {
        public class Query : IRequest<Response>
        {
            public Query(int studentId)
            {
                StudentId = studentId;
            }

            public int StudentId { get; set; }

        }

        public class Response
        {
            public Response(GroupProfileResponseDto @group)
            {
                Group = @group;
            }

            public GroupProfileResponseDto Group { get; set; }
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
                GroupProfileResponseDto result = await _context
                    .Students
                    .Where(sgm => sgm.Id == request.StudentId)
                    .Select(sgm => sgm.Group)
                    .Select(GroupProfileResponseDto.FromEntity)
                    .SingleOrDefaultAsync();

                return new Response(result);
            }
        }
    }
}