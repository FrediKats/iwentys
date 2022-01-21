using AutoMapper;
using AutoMapper.QueryableExtensions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByName
{
    public record Query(string GroupName) : IRequest<Response>;
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
            var name = new GroupName(request.GroupName);
            var result = await _context
                .StudyGroups
                .Where(StudyGroup.IsMatch(name))
                .ProjectTo<StudyGroupProfileResponseDto>(_mapper.ConfigurationProvider)
                .SingleAsync();

            return new Response(result);
        }
    }
}