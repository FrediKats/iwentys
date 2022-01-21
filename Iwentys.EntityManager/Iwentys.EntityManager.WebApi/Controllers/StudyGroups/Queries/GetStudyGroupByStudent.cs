using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByStudent
{
    public record Query(int StudentId) : IRequest<Response>;
    public record Response(StudyGroupProfileResponseDto StudyGroup);

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
            StudyGroupProfileResponseDto? result = await _context
                .Students
                .Where(sgm => sgm.Id == request.StudentId)
                .Select(sgm => sgm.Group)
                .ProjectTo<StudyGroupProfileResponseDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return new Response(result);
        }
    }
}