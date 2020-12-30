using System.Collections.Generic;
using System.Linq;
using FluentResults;
using Iwentys.Common.Databases;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study;
using Iwentys.Features.Study.Entities;
using Iwentys.Integrations.GoogleTableIntegration;
using Iwentys.Integrations.GoogleTableIntegration.Marks;
using Microsoft.Extensions.Logging;

namespace Iwentys.Endpoint.Server.Source.BackgroundServices
{
    public class MarkGoogleTableUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<SubjectActivityEntity> _subjectActivityRepository;
        private readonly ILogger _logger;
        private readonly TableParser _tableParser;

        public MarkGoogleTableUpdateService(ILogger logger, string serviceToken, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _tableParser = TableParser.Create(_logger, serviceToken);
            
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _subjectActivityRepository = _unitOfWork.GetRepository<SubjectActivityEntity>();
        }

        public void UpdateSubjectActivityForGroup(GroupSubjectEntity groupSubjectData)
        {
            Result<GoogleTableData> googleTableData = groupSubjectData.TryGetGoogleTableDataConfig();
            if (googleTableData.IsFailed)
            {
                _logger.LogError(googleTableData.ToString());
                return;
            }

            List<SubjectActivityEntity> activities = _subjectActivityRepository.Get().ToList();

            foreach (StudentSubjectScore subjectScore in _tableParser.Execute(new MarkParser(googleTableData.Value, _logger)))
            {
                SubjectActivityEntity activity = activities
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

                    StudentEntity studentProfile = _studentRepository
                        .Get()
                        .FirstOrDefault(s => subjectScore.Name.Contains(s.FirstName)
                                    && subjectScore.Name.Contains(s.SecondName));

                    if (studentProfile is null)
                    {
                        _logger.LogWarning($"Student wsa not found: student:{subjectScore.Name}");
                        continue;
                    }

                    //TODO: remove wait
                    _subjectActivityRepository.InsertAsync(new SubjectActivityEntity
                    {
                        StudentId = studentProfile.Id,
                        GroupSubjectEntityId = groupSubjectData.Id,
                        Points = pointsCount
                    }).Wait();
                    _unitOfWork.CommitAsync().Wait();
                    
                    continue;
                }

                activity.Points = pointsCount;
                _subjectActivityRepository.Update(activity);
                _unitOfWork.CommitAsync().Wait();
            }
        }

        private static bool IsMatchedWithStudent(StudentSubjectScore ss, StudentEntity student)
        {
            return ss.Name.Contains(student.FirstName)
                   && ss.Name.Contains(student.SecondName);
        }
    }
}