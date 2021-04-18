using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Study;
using Iwentys.Features.Study;
using Iwentys.Integrations.GoogleTableIntegration;
using Iwentys.Integrations.GoogleTableIntegration.Marks;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Server.Source.BackgroundServices
{
    public class MarkGoogleTableUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<SubjectActivity> _subjectActivityRepository;
        private readonly ILogger _logger;
        private readonly TableParser _tableParser;

        public MarkGoogleTableUpdateService(ILogger logger, string serviceToken, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _tableParser = TableParser.Create(_logger, serviceToken);
            
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _subjectActivityRepository = _unitOfWork.GetRepository<SubjectActivity>();
        }

        public async Task UpdateSubjectActivityForGroup(GroupSubject groupSubjectData)
        {
            Result<GoogleTableData> googleTableData = groupSubjectData.TryGetGoogleTableDataConfig();
            if (googleTableData.IsFailed)
            {
                _logger.LogError(googleTableData.ToString());
                return;
            }

            List<SubjectActivity> activities = _subjectActivityRepository.Get().ToList();

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

                    Student studentProfile = _studentRepository
                        .Get()
                        .FirstOrDefault(s => subjectScore.Name.Contains(s.FirstName)
                                    && subjectScore.Name.Contains(s.SecondName));

                    if (studentProfile is null)
                    {
                        _logger.LogWarning($"Student wsa not found: student:{subjectScore.Name}");
                        continue;
                    }

                    _subjectActivityRepository.Insert(new SubjectActivity
                    {
                        StudentId = studentProfile.Id,
                        GroupSubjectId = groupSubjectData.Id,
                        Points = pointsCount
                    });
                    await _unitOfWork.CommitAsync();
                    
                    continue;
                }

                activity.Points = pointsCount;
                _subjectActivityRepository.Update(activity);
                await _unitOfWork.CommitAsync();
            }
        }

        private static bool IsMatchedWithStudent(StudentSubjectScore ss, Student student)
        {
            return ss.Name.Contains(student.FirstName)
                   && ss.Name.Contains(student.SecondName);
        }
    }
}