using AutoMapper;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetSubjectById
{
    public record Query(int SubjectId) : IRequest<Response>;
    public record Response(SubjectProfileDto Subject);


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
            SubjectProfileDto result = await _mapper
                .ProjectTo<SubjectProfileDto>(_context.Subjects)
                .FirstAsync(s => s.Id == request.SubjectId);

            return new Response(result);
        }
    }
}