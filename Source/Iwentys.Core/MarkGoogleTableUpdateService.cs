using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Iwentys.Database.Context;
using Iwentys.Integrations.GoogleTableIntegration;
using Iwentys.Integrations.GoogleTableIntegration.Marks;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Study;
using Iwentys.Models.Types;
using Microsoft.Extensions.Logging;

namespace Iwentys.Core
{
    public class MarkGoogleTableUpdateService
    {
        private readonly ILogger _logger;
        private readonly TableParser _tableParser;
        private readonly DatabaseAccessor _databaseAccessor;


        public MarkGoogleTableUpdateService(ILogger logger, DatabaseAccessor databaseAccessor, string serviceToken)
        {
            _logger = logger;
            _databaseAccessor = databaseAccessor;
            _tableParser = TableParser.Create(_logger, serviceToken);
        }

        public void UpdateSubjectActivityForGroup(GroupSubjectEntity groupSubjectData)
        {
            Result<GoogleTableData> googleTableData = groupSubjectData.TryGetGoogleTableDataConfig();
            if (googleTableData.IsFailed)
            {
                _logger.LogError(googleTableData.ToString());
                return;
            }

            List<SubjectActivityEntity> activities = _databaseAccessor.SubjectActivity.Read().ToList();

            foreach (StudentSubjectScore subjectScore in _tableParser.Execute(new MarkParser(googleTableData.Value, _logger)))
            {
                SubjectActivityEntity activity = activities
                    .SingleOrDefault(s => subjectScore.IsMatchedWithStudent(s.Student)
                                          && s.GroupSubject.SubjectId == groupSubjectData.SubjectId);

                if (!Tools.ParseInAnyCulture(subjectScore.Score, out double pointsCount))
                {
                    pointsCount = 0;
                    _logger.LogWarning($"Cannot parse value: student:{subjectScore.Name}, subjectId:{groupSubjectData.SubjectId}, groupId:{groupSubjectData.StudyGroupId}");
                }

                if (activity == null)
                {
                    _logger.LogWarning($"Subject info was not found: student:{subjectScore.Name}, subjectId:{groupSubjectData.SubjectId}, groupId:{groupSubjectData.StudyGroupId}");

                    StudentEntity studentProfile = _databaseAccessor.Student
                        .Read()
                        .FirstOrDefault(s => subjectScore.Name.Contains(s.FirstName)
                                    && subjectScore.Name.Contains(s.SecondName));

                    if (studentProfile is null)
                    {
                        _logger.LogWarning($"Student wsa not found: student:{subjectScore.Name}");
                        continue;
                    }

                    _databaseAccessor.SubjectActivity.Create(new SubjectActivityEntity
                    {
                        StudentId = studentProfile.Id,
                        GroupSubjectEntityId = groupSubjectData.Id,
                        Points = pointsCount
                    });

                    continue;
                }

                activity.Points = pointsCount;
                _databaseAccessor.SubjectActivity.Update(activity);
            }
        }
    }
}