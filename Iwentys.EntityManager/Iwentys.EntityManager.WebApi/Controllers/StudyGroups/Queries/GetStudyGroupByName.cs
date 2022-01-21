using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByName
{
    public record Query(string GroupName) : IRequest<Response>;
    public record Response(GroupProfileResponseDto Group);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
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