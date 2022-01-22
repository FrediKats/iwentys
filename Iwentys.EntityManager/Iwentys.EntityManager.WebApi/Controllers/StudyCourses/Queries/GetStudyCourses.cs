using AutoMapper;
using Iwentys.EntityManager.DataAccess;
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
        private readonly IMapper _mapper;

        public Handler(IwentysEntityManagerDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<StudyCourseInfoDto> result = await _context
                .StudyCourses
                .Select(c => new StudyCourseInfoDto{CourseId = c.Id, CourseTitle = c.StudyProgram.Name + " " + c.GraduationYear})
                .ToListAsync();

            return new Response(result);
        }
    }
}