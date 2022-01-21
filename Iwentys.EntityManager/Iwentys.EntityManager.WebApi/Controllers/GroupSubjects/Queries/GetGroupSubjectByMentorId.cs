using Iwentys.EntityManager.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetGroupSubjectByMentorId
{
    public record Query(int? MentorId) : IRequest<Response>;
    public record Response(List<GroupSubjectInfoDto> Groups);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GroupSubjectInfoDto> result = await _context
                .GroupSubjects
                .WhereIf(request.MentorId, gs => gs.Mentors.Any(m => m.UserId == request.MentorId))
                .Select(GroupSubjectInfoDto.FromEntity)
                .ToListAsync();

            return new Response(result);
        }
    }
}