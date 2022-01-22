using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;
using Iwentys.EntityManagerServiceIntegration;

namespace Iwentys.Gamification;

public class StudyLeaderboardRowDtoWithoutStudent
{
    public int StudentId { get; init; }
    public double Activity { get; init; }

    public StudyLeaderboardRowDtoWithoutStudent(List<SubjectActivity> activity)
        : this(activity.First().StudentId, activity.Sum(a => a.Points))
    {
    }

    public StudyLeaderboardRowDtoWithoutStudent(int studentId, double activity)
    {
        StudentId = studentId;
        Activity = activity;
    }
}

public class StudyLeaderboardRowDto
{
    public StudentInfoDto Student { get; init; }
    public double Activity { get; init; }

    public StudyLeaderboardRowDto(Student student, int githubActivity)
        : this(EntityManagerApiDtoMapper.Map(student), githubActivity)
    {
    }

    public StudyLeaderboardRowDto(StudentInfoDto student, double activity)
    {
        Student = student;
        Activity = activity;
    }

    public StudyLeaderboardRowDto()
    {
    }
}