using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement.Mentors.Dto;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Extensions;
using Iwentys.Infrastructure.Application.Filters;
using Iwentys.Modules.AccountManagement.Dtos.Mentors;
using Iwentys.Modules.AccountManagement.Mentors.Commands;
using Iwentys.Modules.AccountManagement.Mentors.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Iwentys.Modules.AccountManagement.Mentors
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
        public async Task<ActionResult<IReadOnlyList<SubjectMentorsDto>>> GetAll(
            [FromQuery] int takeAmount,
            [FromQuery] int pageNumber)
        {
            AuthorizedUser authorizedUser = this.TryAuthWithToken();
            var subjectsMentors = await _mediator.Send(new GetAllSubjectsMentors.Query(authorizedUser));

            var paginationFilter = new PaginationFilter(takeAmount, pageNumber);

            return Ok(IndexViewModelExtensions<SubjectMentorsDto>
                .ToIndexViewModel(subjectsMentors.SubjectMentors.ToList(), paginationFilter));
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