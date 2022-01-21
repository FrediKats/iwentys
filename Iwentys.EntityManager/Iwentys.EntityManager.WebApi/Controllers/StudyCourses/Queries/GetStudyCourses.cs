using System.Linq.Expressions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyCourses
{
    public record Query : IRequest<Response>;
    public record Response(List<StudyCourseInfoDto> Courses);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<StudyCourseInfoDto> result = await _context
                .StudyCourses
                .Select(entity => new StudyCourseInfoDto
                {
                    CourseId = entity.Id,
                    CourseTitle = entity.StudyProgram.Name + " " + entity.GraduationYear
                })
                .ToListAsync();

            return new Response(result);
        }
    }
}