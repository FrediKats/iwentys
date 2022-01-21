using System.Linq.Expressions;
using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
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

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GroupSubjectInfoDto> result = await _context
                .GroupSubjects
                .WhereIf(request.MentorId, gs => gs.Mentors.Any(m => m.UserId == request.MentorId))
                .Select(entity => new GroupSubjectInfoDto
                {
                    Subject = new SubjectProfileDto
                    {
                        Id = entity.Subject.Id,
                        Name = entity.Subject.Title
                    },
                    StudyGroup = new GroupProfileResponsePreviewDto
                    {
                        Id = entity.StudyGroup.Id,
                        GroupName = entity.StudyGroup.GroupName,
                        GroupAdminId = entity.StudyGroup.GroupAdminId,
        },
                    TableLink = entity.TableLink,
                })
                .ToListAsync();

            return new Response(result);
        }
    }
}