using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.WebService.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.AccountManagement
{
    [Route("api/account-management/mentors")]
    [ApiController]
    public class MentorsManagementController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MentorsManagementController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetAll))]
        public async Task<ActionResult<IReadOnlyList<SubjectMentorsDto>>> GetAll()
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            var subjectsMentors = await _mediator.Send(new GetAllSubjectsMentors.Query(authorizedUser));
            return Ok(subjectsMentors.SubjectMentors);
        }

        [HttpGet("by-group-subject/{id}")]
        public async Task<ActionResult<GroupMentorsDto>> GetByGroupSubject(int id)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            var groupMentors = await _mediator.Send(new GetMentorsByGroupSubjectId.Query(authorizedUser,id));
            return Ok(groupMentors.GroupMentors);
        }

        [HttpDelete(nameof(RemoveMentorFromGroup))]
        public async Task<ActionResult> RemoveMentorFromGroup([FromQuery] int groupSubjectId, [FromQuery] int mentorId)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _mediator.Send(new RemoveMentorFromGroup.Command(authorizedUser, groupSubjectId, mentorId));
            return Ok();
        }

        [HttpPost(nameof(AddMentor))]
        public async Task<ActionResult> AddMentor([FromBody] SubjectMentorCreateArgs args)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            await _mediator.Send(new AddMentor.Command(authorizedUser, args));
            return Ok();
        }
    }
}