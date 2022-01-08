using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement
{
    public class AddMentor
    {
        public class Command : IRequest
        {
            public AuthorizedUser AuthorizedUser { get; set; }
            public SubjectMentorCreateArgs Args { get; set; }

            public Command(AuthorizedUser authorizedUser, SubjectMentorCreateArgs args)
            {
                AuthorizedUser = authorizedUser;
                Args = args;
            }
        }
        
        public class Handler : IRequestHandler<Command>
        {
            private readonly IwentysDbContext _dbContext;

            public Handler(IwentysDbContext context)
            {
                _dbContext = context;
            }
                
            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.IwentysUsers.GetById(request.AuthorizedUser.Id);

                if (!user.IsAdmin)
                    throw InnerLogicException.NotEnoughPermissionFor(user.Id);
                
                var practiceMentor = await _dbContext.IwentysUsers.FirstOrDefaultAsync(
                    s => s.Id == request.Args.MentorId,cancellationToken);

                if (practiceMentor is null)
                    throw new ArgumentException("Invalid mentor", nameof(request.Args.MentorId));
                
                var groupSubjects = await _dbContext.GroupSubjects.Where(
                    gs=>gs.SubjectId == request.Args.SubjectId 
                        && request.Args.GroupSubjectIds.Contains(gs.StudyGroupId))
                                                       .ToListAsync(cancellationToken);
                
                foreach (var groupSubject in groupSubjects)
                {
                    if (groupSubject.Mentors.Any(m=> !m.IsLector && m.UserId==request.Args.MentorId))
                        continue;
                    
                    groupSubject.Mentors.Add(new GroupSubjectMentor()
                    {
                        IsLector = false,
                        GroupSubjectId = groupSubject.Id,
                        UserId = request.Args.MentorId
                    });

                    _dbContext.GroupSubjects.Update(groupSubject);
                }

                await _dbContext.SaveChangesAsync(cancellationToken);
                
                return Unit.Value;
            }
        }
        
    }
}