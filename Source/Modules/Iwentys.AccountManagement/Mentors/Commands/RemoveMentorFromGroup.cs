using System;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common;
using Iwentys.DataAccess;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.AccountManagement;

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
        private readonly IwentysDbContext _context;
        private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

        public Handler(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
        {
            _context = context;
            _entityManagerApiClient = entityManagerApiClient;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _entityManagerApiClient.IwentysUserProfiles.GetByIdAsync(request.AuthorizedUser.Id);

            if (!user.IsAdmin)
                throw InnerLogicException.NotEnoughPermissionFor(user.Id);
                
            var groupSubjectMentor = await _context.GroupSubjectMentors.FirstOrDefaultAsync(gsm =>
                gsm.UserId == request.MentorId 
                && gsm.GroupSubjectId == request.GroupSubjectId 
                && !gsm.IsLector,cancellationToken);

            if (groupSubjectMentor is null)
                throw new ArgumentException("User is not mentor", nameof(request));

            _context.GroupSubjectMentors.Remove(groupSubjectMentor);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}