using System.Linq.Expressions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public class GetStudyGroupByStudent
{
    public record Query(int StudentId) : IRequest<Response>;
    public record Response(GroupProfileResponseDto Group);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            GroupProfileResponseDto result = await _context
                .Students
                .Where(sgm => sgm.Id == request.StudentId)
                .Select(sgm => sgm.Group)
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
                .SingleOrDefaultAsync();

            return new Response(result);
        }
    }
}