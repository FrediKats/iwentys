using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
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
        private IConfiguration _configuration;

        public DebugCommandController(ISubjectActivityRepository subjectActivityRepository, ISubjectForGroupRepository subjectForGroupRepository, IConfiguration configuration)
        {
            _subjectActivityRepository = subjectActivityRepository;
            _subjectForGroupRepository = subjectForGroupRepository;
            _configuration = configuration;
        }

        public void UpdateSubjectActivityData(SubjectActivity activity)
        {
            _subjectActivityRepository.Update(activity);
        }

        public void UpdateSubjectActivityForGroup(int subjectId, int groupId)
        {
            var subjectData = _subjectForGroupRepository
                .Read().FirstOrDefault(s => s.SubjectId == subjectId && s.StudyGroupId == groupId);
            if (subjectData == null)
            {
                // TODO: Some logs
                return;
            }

            var googleTableData = subjectData.GetGoogleTableDataConfig;

            var credential = GoogleCredential.FromJson(_configuration["GoogleTable:Credentials"]).
                CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            var sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "IwentysTableParser",
            });

            var tableParser = new TableParser(sheetsService, googleTableData);

            foreach (var student in tableParser.GetStudentsList())
            {
                // Это очень плохая проверка, но я пока не придумал,
                // как по-другому сопоставлять данные с гугл-таблицы со студентом
                // TODO: Сделать нормальную проверку
                var activity = _subjectActivityRepository.Read().FirstOrDefault(s =>
                    student.Name.Contains(s.Student.FirstName) && student.Name.Contains(s.Student.SecondName) &&
                    s.SubjectForGroupId == subjectData.Id);
                if (activity == null)
                {
                    // TODO: Some logs
                    return;
                }
                activity.Points = (int)double.Parse(student.Score);
                UpdateSubjectActivityData(activity);
            }
        }
    }
}
