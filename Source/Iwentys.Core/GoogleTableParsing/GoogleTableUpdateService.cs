using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.Extensions.Configuration;

namespace Iwentys.Core.GoogleTableParsing
{
    public class GoogleTableUpdateService
    {
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        private readonly IConfiguration _configuration;

        public GoogleTableUpdateService(ISubjectActivityRepository subjectActivityRepository, IConfiguration configuration)
        {
            _subjectActivityRepository = subjectActivityRepository;
            _configuration = configuration;
        }

        public void UpdateSubjectActivityForGroup(SubjectForGroup subjectData)
        {
            GoogleTableData googleTableData = subjectData.GetGoogleTableDataConfig();

            GoogleCredential credential = GoogleCredential
                .FromJson(_configuration["GoogleTable:Credentials"])
                .CreateScoped(SheetsService.Scope.SpreadsheetsReadonly);

            var sheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "IwentysTableParser",
            });

            var tableParser = new TableParser(sheetsService, googleTableData);

            foreach (StudentSubjectScore student in tableParser.GetStudentsList())
            {
                // Это очень плохая проверка, но я пока не придумал,
                // как по-другому сопоставлять данные с гугл-таблицы со студентом
                // TODO: Сделать нормальную проверку
                SubjectActivity activity = _subjectActivityRepository
                    .Read()
                    .FirstOrDefault(s => student.Name.Contains(s.Student.FirstName)
                                         && student.Name.Contains(s.Student.SecondName)
                                         && s.SubjectForGroupId == subjectData.Id);
                if (activity == null)
                {
                    // TODO: Some logs
                    return;
                }

                activity.Points = double.Parse(student.Score);
                _subjectActivityRepository.Update(activity);
            }
        }
    }
}