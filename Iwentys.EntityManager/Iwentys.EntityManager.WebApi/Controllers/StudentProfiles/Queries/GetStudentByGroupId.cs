using AutoMapper;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public static class GetStudentByGroupId
{
    public record Query(int GroupId) : IRequest<Response>;
    public record Response(IReadOnlyCollection<StudentInfoDto> Students);

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
            List<StudentInfoDto> result = await _mapper
                .ProjectTo<StudentInfoDto>(_context.Students)
                .Where(s => s.GroupId == request.GroupId)
                .ToListAsync(cancellationToken: cancellationToken);

            return new Response(result);
        }
    }
}