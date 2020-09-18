using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.GoogleTableParsing
{
    public class GoogleTableUpdateService
    {
        private readonly ILogger _logger;
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        private readonly IStudentRepository _studentRepository;

        public GoogleTableUpdateService(ILogger logger, ISubjectActivityRepository subjectActivityRepository, IStudentRepository studentRepository)
        {
            _logger = logger;
            _subjectActivityRepository = subjectActivityRepository;
            _studentRepository = studentRepository;
        }

        public void UpdateSubjectActivityForGroup(GroupSubjectEntity groupSubjectData)
        {
            GoogleTableData googleTableData = groupSubjectData.GetGoogleTableDataConfig();
            //TODO: remove this hack
            if (googleTableData is null)
                return;

            SheetsService sheetsService = GetServiceForApiToken();

            var tableParser = new TableParser(_logger, sheetsService, googleTableData);

            foreach (StudentSubjectScore student in tableParser.GetStudentsList())
            {
                // Это очень плохая проверка, но я пока не придумал,
                // как по-другому сопоставлять данные с гугл-таблицы со студентом
                // TODO: Сделать нормальную проверку
                SubjectActivityEntity activity = _subjectActivityRepository
                    .Read()
                    .SingleOrDefault(s => student.Name.Contains(s.Student.FirstName)
                                          && student.Name.Contains(s.Student.SecondName)
                                          && s.GroupSubjectEntity.SubjectId == groupSubjectData.SubjectId);

                if (!Tools.ParseInAnyCulture(student.Score, out double pointsCount))
                {
                    pointsCount = 0;
                    _logger.LogWarning($"Cannot parse value: student:{student.Name}, subjectId:{groupSubjectData.SubjectId}, groupId:{groupSubjectData.StudyGroupId}");
                }

                if (activity == null)
                {
                    _logger.LogWarning($"Subject info was not found: student:{student.Name}, subjectId:{groupSubjectData.SubjectId}, groupId:{groupSubjectData.StudyGroupId}");

                    StudentEntity studentProfile = _studentRepository
                        .Read()
                        .FirstOrDefault(s => student.Name.Contains(s.FirstName)
                                    && student.Name.Contains(s.SecondName));

                    if (studentProfile is null)
                    {
                        _logger.LogWarning($"Student wsa not found: student:{student.Name}");
                        continue;
                    }

                    _subjectActivityRepository.Create(new SubjectActivityEntity
                    {
                        StudentId = studentProfile.Id,
                        GroupSubjectEntityId = groupSubjectData.StudyGroupId,
                        Points = pointsCount
                    });

                    continue;
                }

                activity.Points = pointsCount;
                _subjectActivityRepository.Update(activity);
            }
        }

        private SheetsService GetServiceForCredential()
        {
            GoogleCredential credential = GoogleCredential
                .FromJson(ApplicationOptions.GoogleServiceToken)
                .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            return new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "IwentysTableParser",
                HttpClientInitializer = credential,
            });
        }

        private SheetsService GetServiceForApiToken()
        {
            return new SheetsService(new BaseClientService.Initializer()
            {
                ApplicationName = "IwentysTableParser",
                ApiKey = ApplicationOptions.GoogleServiceToken
            });
        }
    }
}