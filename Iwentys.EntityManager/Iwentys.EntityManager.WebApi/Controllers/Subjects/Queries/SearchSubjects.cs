using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class SearchSubjects
{
    public record Query(StudySearchParametersDto SearchParametersDto) : IRequest<Response>;
    public record Response(List<SubjectProfileDto> Subjects);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<SubjectProfileDto> result = await _context
                .GroupSubjects
                .SearchSubjects(request.SearchParametersDto)
                .Select(entity => new SubjectProfileDto(entity.Id, entity.Title))
                .ToListAsync();

            return new Response(result);
        }
    }
}