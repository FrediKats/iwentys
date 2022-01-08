using System.Linq;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.Application.BackgroundServices;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.Application.Options;
using Iwentys.Infrastructure.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Exceptional;

namespace Iwentys.Endpoints.Api.Controllers
{
    [Route("api/DebugCommand")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly MarkGoogleTableUpdateService _markGoogleTableUpdateService;
        private readonly GithubIntegrationService _githubIntegrationService;
        private readonly ILogger<DebugCommandController> _logger;
        private readonly IwentysDbContext _context;

        public DebugCommandController(ILogger<DebugCommandController> logger, TokenApplicationOptions tokenApplicationOptions, IwentysDbContext context, GithubIntegrationService githubIntegrationService)
        {
            _logger = logger;
            _context = context;

            _githubIntegrationService = githubIntegrationService;
            _markGoogleTableUpdateService = new MarkGoogleTableUpdateService(_logger, tokenApplicationOptions.GoogleServiceToken, _context);
        }

        //[HttpPost("UpdateSubjectActivityData")]
        //public void UpdateSubjectActivityData(SubjectActivity activity)
        //{
        //    _databaseAccessor.SubjectActivity.UpdateAsync(activity);
        //}

        [HttpPost("UpdateSubjectActivityForGroup")]
        public async Task<ActionResult> UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            GroupSubject groupSubjectData = _context
                .GroupSubjects
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (groupSubjectData is null)
            {
                _logger.LogWarning($"Subject info was not found: subjectId:{subjectId}, groupId:{groupId}");
                return Ok();
            }

            await _markGoogleTableUpdateService.UpdateSubjectActivityForGroup(groupSubjectData);
            return Ok();
        }

        [HttpGet("update-guild-impact")]
        public async Task<ActionResult> UpdateGuildImpact()
        {
            var guildMembers = await _context.GuildMembers
                .Where(GuildMember.IsMember())
                .ToListAsync();

            foreach (GuildMember member in guildMembers)
            {
                ContributionFullInfo contributionFullInfo = await _githubIntegrationService.User.FindUserContributionOrEmpty(member.Member);
                member.MemberImpact = contributionFullInfo.Total;
            }

            _context.GuildMembers.UpdateRange(guildMembers);
            await _context.SaveChangesAsync();
            return Ok();
        }

        //[HttpGet("teachers")]
        //public ActionResult<List<SubjectTeacherInfo>> LoadTeachers([FromQuery] string tableId, [FromQuery] string range)
        //{
        //    var tableParser = TableParser.Create(_logger);
        //    var subjectTeacherParser = new SubjectTeacherParser(tableId, range);
        //    List<SubjectTeacherInfo> result = tableParser.Execute(subjectTeacherParser);
        //    return Ok(result);
        //}

        [HttpGet("~/errors/log/{path?}/{subPath?}", Name = "ErrorLog")]
        public async Task Exceptions() => await ExceptionalMiddleware.HandleRequestAsync(HttpContext);
    }
}
