using Iwentys.EntityManager.DataAccess;
using Iwentys.EntityManager.Domain;
using Iwentys.EntityManager.WebApiDtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.EntityManager.WebApi;

public static class AddTeacher
{
    public record Command(SubjectTeacherCreateArgs Args) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IwentysEntityManagerDbContext _context;

        public Handler(IwentysEntityManagerDbContext context)
        {
            _context = context;
        }
                
        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            List<GroupSubject> groupSubjects = await _context.GroupSubjects.Where(
                    gs => gs.SubjectId == request.Args.SubjectId
                          && request.Args.GroupSubjectIds.Contains(gs.StudyGroupId))
                .ToListAsync(cancellationToken);
                
            foreach (GroupSubject groupSubject in groupSubjects)
            {
                if (groupSubject.Teachers.Any(m => m.TeacherType == request.Args.TeacherType && m.TeacherId == request.Args.TeacherId))
                    continue;
                    
                groupSubject.Teachers.Add(new GroupSubjectTeacher()
                {
                    TeacherType = request.Args.TeacherType,
                    GroupSubjectId = groupSubject.Id,
                    TeacherId = request.Args.TeacherId
                });

                _context.GroupSubjects.Update(groupSubject);
            }

            await _context.SaveChangesAsync(cancellationToken);
                
            return Unit.Value;
        }
    }
}