using Iwentys.DataAccess;
using Iwentys.Domain.Study;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.EntityManagerServiceIntegration;

public class EntityManagerDatabaseSynchronization
{
    private readonly IwentysDbContext _context;
    private readonly TypedIwentysEntityManagerApiClient _entityManagerApiClient;

    public EntityManagerDatabaseSynchronization(IwentysDbContext context, TypedIwentysEntityManagerApiClient entityManagerApiClient)
    {
        _context = context;
        _entityManagerApiClient = entityManagerApiClient;
    }

    public async Task UpdateStudentGroup()
    {
        Dictionary<int, int> groupToCourseMap = new Dictionary<int, int>();
        IReadOnlyCollection<StudyCourseInfoDto> courses = await _entityManagerApiClient.Client.StudyCourses.StudyCoursesAsync();
        foreach (StudyCourseInfoDto course in courses)
        {
            IReadOnlyCollection<StudyGroupProfileResponseDto> groups = await _entityManagerApiClient.Client.StudyGroups.GetByCourseIdAsync(course.CourseId);
            foreach (StudyGroupProfileResponseDto @group in groups)
            {
                groupToCourseMap[group.Id] = course.CourseId;
            }
        }

        IReadOnlyCollection<Student> students = await _entityManagerApiClient.StudentProfiles.GetAsync();
        HashSet<int> addedToDbStudents = _context.StudentPositions.Select(s => s.StudentId).ToHashSet();

        foreach (Student student in students)
        {
            if (addedToDbStudents.Contains(student.Id))
                continue;

            if (student.GroupId is null)
                continue;

            _context.StudentPositions.Add(new StudentPosition { StudentId = student.Id, GroupId = student.GroupId.Value, CourseId = groupToCourseMap[student.GroupId.Value] });
        }

        await _context.SaveChangesAsync();
    }

}