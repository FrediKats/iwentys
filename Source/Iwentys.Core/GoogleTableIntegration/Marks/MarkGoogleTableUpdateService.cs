using System.Linq;
using FluentResults;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core.GoogleTableIntegration.Marks
{
    public class MarkGoogleTableUpdateService
    {
        private readonly ILogger _logger;
        private readonly ISubjectActivityRepository _subjectActivityRepository;
        private readonly IStudentRepository _studentRepository;

        public MarkGoogleTableUpdateService(ILogger logger, ISubjectActivityRepository subjectActivityRepository, IStudentRepository studentRepository)
        {
            _logger = logger;
            _subjectActivityRepository = subjectActivityRepository;
            _studentRepository = studentRepository;
        }

        public void UpdateSubjectActivityForGroup(GroupSubjectEntity groupSubjectData)
        {
            Result<GoogleTableData> googleTableData = groupSubjectData.TryGetGoogleTableDataConfig();
            if (googleTableData.IsFailed)
            {
                _logger.LogError(googleTableData.ToString());
                return;
            }

            var tableParser = TableParser.Create(_logger);
            foreach (StudentSubjectScore student in tableParser.Execute(new MarkParser(googleTableData.Value, _logger)))
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
    }
}