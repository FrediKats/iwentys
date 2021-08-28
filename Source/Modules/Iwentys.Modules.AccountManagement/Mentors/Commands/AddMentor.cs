using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement.Mentors.Dto;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.AccountManagement.Mentors.Commands
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
                
                foreach (var groupId in request.Args.GroupSubjectIds)
                {
                    var groupSubject = await _dbContext.GroupSubjects.FirstOrDefaultAsync(
                        gs=>gs.SubjectId==request.Args.SubjectId && gs.StudyGroupId==groupId);
                    if (groupSubject is null || 
                        groupSubject.PracticeMentors.Any(m=>m.UserId==request.Args.MentorId))
                        continue;
                    groupSubject.PracticeMentors.Add(new GroupSubjectMentor()
                    {
                        GroupSubjectId = groupSubject.Id,
                        UserId = request.Args.MentorId
                    });

                    _dbContext.GroupSubjects.Update(groupSubject);
                }

                await _dbContext.SaveChangesAsync();
                
                return Unit.Value;
            }
        }
        
    }
}