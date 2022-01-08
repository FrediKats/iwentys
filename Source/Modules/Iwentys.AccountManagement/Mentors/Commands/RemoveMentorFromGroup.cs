using System;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.AccountManagement.Mentors.Commands
{
    public class RemoveMentorFromGroup
    {
        public class Command : IRequest
        {
            public AuthorizedUser AuthorizedUser { get; set; }
            public int GroupSubjectId { get; set; }
            public int MentorId { get; set; }

            public Command(AuthorizedUser authorizedUser, int groupSubjectId, int mentorId)
            {
                AuthorizedUser = authorizedUser;
                GroupSubjectId = groupSubjectId;
                MentorId = mentorId;
            }
        }
        
        public class Handler : IRequestHandler<Command>
        {
            private readonly IwentysDbContext _dbContext;

            public Handler(IwentysDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.IwentysUsers.GetById(request.AuthorizedUser.Id);

                if (!user.IsAdmin)
                    throw InnerLogicException.NotEnoughPermissionFor(user.Id);
                
                var groupSubjectMentor = await _dbContext.GroupSubjectMentors.FirstOrDefaultAsync(gsm =>
                    gsm.UserId == request.MentorId 
                    && gsm.GroupSubjectId == request.GroupSubjectId 
                    && !gsm.IsLector,cancellationToken);

                if (groupSubjectMentor is null)
                    throw new ArgumentException("User is not mentor", nameof(request));

                _dbContext.GroupSubjectMentors.Remove(groupSubjectMentor);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}