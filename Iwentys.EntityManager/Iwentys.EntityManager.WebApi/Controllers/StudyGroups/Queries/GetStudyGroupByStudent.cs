using Iwentys.EntityManager.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByStudent
{
    public record Query(int StudentId) : IRequest<Response>;
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