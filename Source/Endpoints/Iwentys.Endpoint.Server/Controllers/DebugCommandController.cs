using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.Study;
using Iwentys.Endpoint.Server.Source.BackgroundServices;
using Iwentys.Endpoint.Server.Source.Options;
using Iwentys.Features.Guilds.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Exceptional;

namespace Iwentys.Endpoint.Server.Controllers
{
    [Route("api/DebugCommand")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly MarkGoogleTableUpdateService _markGoogleTableUpdateService;
        private readonly ILogger<DebugCommandController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly GuildService _guildService;

        public DebugCommandController(ILogger<DebugCommandController> logger, IUnitOfWork unitOfWork, TokenApplicationOptions tokenApplicationOptions, GuildService guildService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _guildService = guildService;

            _markGoogleTableUpdateService = new MarkGoogleTableUpdateService(_logger, tokenApplicationOptions.GoogleServiceToken, _unitOfWork);
        }

        //[HttpPost("UpdateSubjectActivityData")]
        //public void UpdateSubjectActivityData(SubjectActivity activity)
        //{
        //    _databaseAccessor.SubjectActivity.UpdateAsync(activity);
        //}

        [HttpPost("UpdateSubjectActivityForGroup")]
        public async Task<ActionResult> UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            GroupSubject groupSubjectData = _unitOfWork.GetRepository<GroupSubject>()
                .Get()
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
            await _guildService.UpdateGuildMemberImpact();
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
