using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Iwentys.Infrastructure.Application.Controllers.Study.Dtos;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class GetStudyGroupByName
    {
        public class Query : IRequest<Response>
        {
            public Query(string groupName)
            {
                GroupName = groupName;
            }

            public string GroupName { get; set; }

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
                var name = new GroupName(request.GroupName);
                var result = await _context
                    .StudyGroups
                    .Where(StudyGroup.IsMatch(name))
                    .Select(GroupProfileResponseDto.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}