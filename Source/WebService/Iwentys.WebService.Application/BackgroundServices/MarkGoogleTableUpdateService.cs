using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Iwentys.EntityManagerServiceIntegration;
using Iwentys.GoogleTableIntegration;
using Iwentys.GoogleTableIntegration.Marks;
using Microsoft.Extensions.Logging;

namespace Iwentys.WebService.Application;

public class MarkGoogleTableUpdateService
{
    private readonly IwentysDbContext _context;
    private readonly ILogger _logger;
    private readonly TableParser _tableParser;
    private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

    public MarkGoogleTableUpdateService(ILogger logger, string serviceToken, IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
    {
        _logger = logger;
        _context = context;
        _entityManagerApiClient = entityManagerApiClient;
        _tableParser = TableParser.Create(_logger, serviceToken);
    }

    public async Task UpdateSubjectActivityForGroup(GroupActivityTable groupActivityTable)
    {
        if (!GoogleTableData.TryCreate(groupActivityTable.SerializedGoogleTableConfig, out var googleTableData))
        {
            return;
        }

        List<SubjectActivity> activities = _context.SubjectActivities.ToList();
        IReadOnlyCollection<Student> students = await _entityManagerApiClient.StudentProfiles.GetAsync();

        foreach (StudentSubjectScore subjectScore in _tableParser.Execute(new MarkParser(googleTableData, _logger)))
        {
            SubjectActivity activity = activities
                .SingleOrDefault(sa => IsMatchedWithStudent(subjectScore, students.Single(s => s.Id == sa.StudentId))
                                      && sa.SubjectId == groupActivityTable.SubjectId);

            if (!Tools.ParseInAnyCulture(subjectScore.Score, out double pointsCount))
            {
                pointsCount = 0;
                _logger.LogWarning($"Cannot parse value: student:{subjectScore.Name}, subjectId:{groupActivityTable.SubjectId}, groupId:{groupActivityTable.GroupId}");
            }

            if (activity is null)
            {
                _logger.LogWarning($"Subject info was not found: student:{subjectScore.Name}, subjectId:{groupActivityTable.SubjectId}, groupId:{groupActivityTable.GroupId}");

                Student studentProfile = students.FirstOrDefault(s => subjectScore.Name.Contains(s.FirstName)
                                                                      && subjectScore.Name.Contains(s.SecondName));

                if (studentProfile is null)
                {
                    _logger.LogWarning($"Student wsa not found: student:{subjectScore.Name}");
                    continue;
                }

                _context.SubjectActivities.Add(new SubjectActivity
                {
                    StudentId = studentProfile.Id,
                    SubjectId = groupActivityTable.SubjectId,
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