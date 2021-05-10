using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Integrations.GoogleTableIntegration;
using Iwentys.Integrations.GoogleTableIntegration.Marks;
using Microsoft.Extensions.Logging;

namespace Iwentys.Infrastructure.Application.Controllers.BackgroundServices
{
    public class MarkGoogleTableUpdateService
    {
        private readonly IwentysDbContext _context;
        private readonly ILogger _logger;
        private readonly TableParser _tableParser;

        public MarkGoogleTableUpdateService(ILogger logger, string serviceToken, IwentysDbContext context)
        {
            _logger = logger;
            _context = context;
            _tableParser = TableParser.Create(_logger, serviceToken);
        }

        public async Task UpdateSubjectActivityForGroup(GroupSubject groupSubjectData)
        {
            Result<GoogleTableData> googleTableData = groupSubjectData.TryGetGoogleTableDataConfig();
            if (googleTableData.IsFailed)
            {
                _logger.LogError(googleTableData.ToString());
                return;
            }

            List<SubjectActivity> activities = _context.SubjectActivities.ToList();

            foreach (StudentSubjectScore subjectScore in _tableParser.Execute(new MarkParser(googleTableData.Value, _logger)))
            {
                SubjectActivity activity = activities
                    .SingleOrDefault(s => IsMatchedWithStudent(subjectScore, s.Student)
                                          && s.GroupSubject.SubjectId == groupSubjectData.SubjectId);

                if (!Integrations.GoogleTableIntegration.Tools.ParseInAnyCulture(subjectScore.Score, out double pointsCount))
                {
                    pointsCount = 0;
                    _logger.LogWarning($"Cannot parse value: student:{subjectScore.Name}, subjectId:{groupSubjectData.SubjectId}, groupId:{groupSubjectData.StudyGroupId}");
                }

                if (activity is null)
                {
                    _logger.LogWarning($"Subject info was not found: student:{subjectScore.Name}, subjectId:{groupSubjectData.SubjectId}, groupId:{groupSubjectData.StudyGroupId}");

                    Student studentProfile = _context
                        .Students
                        .FirstOrDefault(s => subjectScore.Name.Contains(s.FirstName)
                                    && subjectScore.Name.Contains(s.SecondName));

                    if (studentProfile is null)
                    {
                        _logger.LogWarning($"Student wsa not found: student:{subjectScore.Name}");
                        continue;
                    }

                    _context.SubjectActivities.Add(new SubjectActivity
                    {
                        StudentId = studentProfile.Id,
                        GroupSubjectId = groupSubjectData.Id,
                        Points = pointsCount
                    });
                    await _context.SaveChangesAsync();
                    
                    continue;
                }

                activity.Points = pointsCount;
                _context.SubjectActivities.Update(activity);
                await _context.SaveChangesAsync();
            }
        }

        private static bool IsMatchedWithStudent(StudentSubjectScore ss, Student student)
        {
            return ss.Name.Contains(student.FirstName)
                   && ss.Name.Contains(student.SecondName);
        }
    }
}