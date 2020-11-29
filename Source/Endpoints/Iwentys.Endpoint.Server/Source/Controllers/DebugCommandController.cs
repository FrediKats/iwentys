using System.Linq;
using Iwentys.Core;
using Iwentys.Database.Context;
using Iwentys.Models.Entities.Study;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Server.Source.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly MarkGoogleTableUpdateService _markGoogleTableUpdateService;
        private readonly ILogger<DebugCommandController> _logger;
        private readonly DatabaseAccessor _databaseAccessor;

        public DebugCommandController(ILogger<DebugCommandController> logger, DatabaseAccessor databaseAccessor)
        {
            _logger = logger;
            _databaseAccessor = databaseAccessor;

            _markGoogleTableUpdateService = new MarkGoogleTableUpdateService(databaseAccessor.Student, databaseAccessor.SubjectActivity, _logger, ApplicationOptions.GoogleServiceToken);
        }

        //[HttpPost("UpdateSubjectActivityData")]
        //public void UpdateSubjectActivityData(SubjectActivityEntity activity)
        //{
        //    _databaseAccessor.SubjectActivity.UpdateAsync(activity);
        //}

        [HttpPost("UpdateSubjectActivityForGroup")]
        public ActionResult UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            GroupSubjectEntity groupSubjectData = _databaseAccessor.GroupSubject
                .Read()
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (groupSubjectData == null)
            {
                _logger.LogWarning($"Subject info was not found: subjectId:{subjectId}, groupId:{groupId}");
                return Ok();
            }

            _markGoogleTableUpdateService.UpdateSubjectActivityForGroup(groupSubjectData);
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
    }
}
