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

        public void UpdateSubjectActivityForGroup(SubjectForGroup subjectData)
        {
            GoogleTableData googleTableData = subjectData.GetGoogleTableDataConfig();

            SheetsService sheetsService = GetServiceForApiToken();

            var tableParser = new TableParser(_logger, sheetsService, googleTableData);

            foreach (StudentSubjectScore student in tableParser.GetStudentsList())
            {
                // Это очень плохая проверка, но я пока не придумал,
                // как по-другому сопоставлять данные с гугл-таблицы со студентом
                // TODO: Сделать нормальную проверку
                SubjectActivity activity = _subjectActivityRepository
                    .Read()
                    .SingleOrDefault(s => student.Name.Contains(s.Student.FirstName)
                                          && student.Name.Contains(s.Student.SecondName)
                                          && s.SubjectForGroupId == subjectData.Id);

                if (activity == null)
                {
                    _logger.LogWarning($"Subject info was not found: student:{student.Name}, subjectId:{subjectData.SubjectId}, groupId:{subjectData.StudyGroupId}");

                    Student studentProfile = _studentRepository
                        .Read()
                        .FirstOrDefault(s => student.Name.Contains(s.FirstName)
                                    && student.Name.Contains(s.SecondName));

                    if (studentProfile is null)
                    {
                        _logger.LogWarning($"Student wsa not found: student:{student.Name}");
                        continue;
                    }


                    _subjectActivityRepository.Create(new SubjectActivity
                    {
                        StudentId = studentProfile.Id,
                        SubjectForGroupId = subjectData.StudyGroupId,
                        //TODO: support parse for 1.0 and 1,0
                        Points = double.Parse(student.Score)
                    });

                    continue;
                }

                activity.Points = double.Parse(student.Score);
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