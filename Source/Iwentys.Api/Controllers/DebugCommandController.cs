using System.Linq;
using Iwentys.Core.GoogleTableParsing;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Iwentys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebugCommandController : ControllerBase
    {
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        private readonly ISubjectForGroupRepository _subjectForGroupRepository;
        private readonly GoogleTableUpdateService _googleTableUpdateService;

        public DebugCommandController(ISubjectActivityRepository subjectActivityRepository, ISubjectForGroupRepository subjectForGroupRepository, IConfiguration configuration)
        {
            _subjectActivityRepository = subjectActivityRepository;
            _subjectForGroupRepository = subjectForGroupRepository;

            _googleTableUpdateService = new GoogleTableUpdateService(_subjectActivityRepository, configuration);
        }

        [HttpPost("UpdateSubjectActivityData")]
        public void UpdateSubjectActivityData(SubjectActivity activity)
        {
            _subjectActivityRepository.Update(activity);
        }

        [HttpPost("UpdateSubjectActivityForGroup")]
        public void UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            SubjectForGroup subjectData = _subjectForGroupRepository
                .Read()
                .FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);

            if (subjectData == null)
            {
                // TODO: Some logs
                return;
            }
            
            _googleTableUpdateService.UpdateSubjectActivityForGroup(subjectData);
        }
    }
}
