using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.WebApiDtos;
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
        private readonly IMapper _mapper;

        public Handler(IwentysEntityManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GroupSubjectInfoDto> result = await _context
                .GroupSubjects
                .WhereIf(request.MentorId, gs => gs.Teachers.Any(m => m.TeacherId == request.MentorId))
                .ProjectTo<GroupSubjectInfoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new Response(result);
        }
    }
}