using System.Linq.Expressions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByCourseId
{
    public record Query(int? CourseId) : IRequest<Response>;
    public record Response(List<GroupProfileResponseDto> Groups);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GroupProfileResponseDto> result = await _context
                .StudyGroups
                .WhereIf(request.CourseId, gs => gs.StudyCourseId == request.CourseId)
                .Select(entity => new GroupProfileResponseDto
                {
                    Id = entity.Id,
                    GroupName = entity.GroupName,
                    GroupAdminId = entity.GroupAdminId,
                    Students = entity.Students.Select(s => new StudentInfoDto()).ToList(),
                    Subjects = entity.GroupSubjects.Select(gs => new SubjectProfileDto
                    {
                        Id = gs.Subject.Id,
                        Name = gs.Subject.Title
                    }).ToList()
                })
                .ToListAsync();

            return new Response(result);
        }
    }
}