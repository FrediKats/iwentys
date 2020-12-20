using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Endpoint.Server.Source.BackgroundServices;
using Iwentys.Endpoint.Server.Source.Options;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Exceptional;

namespace Iwentys.Endpoint.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly MarkGoogleTableUpdateService _markGoogleTableUpdateService;
        private readonly ILogger<DebugCommandController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public DebugCommandController(ILogger<DebugCommandController> logger, ISubjectActivityRepository subjectActivityRepository, IUnitOfWork unitOfWork, TokenApplicationOptions tokenApplicationOptions)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            
            _markGoogleTableUpdateService = new MarkGoogleTableUpdateService(subjectActivityRepository, _logger, tokenApplicationOptions.GoogleServiceToken, _unitOfWork);
        }

        //[HttpPost("UpdateSubjectActivityData")]
        //public void UpdateSubjectActivityData(SubjectActivityEntity activity)
        //{
        //    _databaseAccessor.SubjectActivity.UpdateAsync(activity);
        //}

        [HttpPost("UpdateSubjectActivityForGroup")]
        public ActionResult UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            GroupSubjectEntity groupSubjectData = _unitOfWork.GetRepository<GroupSubjectEntity>()
                .GetAsync()
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (groupSubjectData is null)
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

        [HttpGet("~/errors/log/{path?}/{subPath?}", Name = "ErrorLog")]
        public async Task Exceptions() => await ExceptionalMiddleware.HandleRequestAsync(HttpContext);
    }
}
