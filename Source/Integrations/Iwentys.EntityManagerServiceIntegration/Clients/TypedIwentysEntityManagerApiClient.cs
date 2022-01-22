using AutoMapper;

namespace Iwentys.EntityManagerServiceIntegration;

public class TypedIwentysEntityManagerApiClient
{
    //public GroupSubjectClient GroupSubjects { get; set; }
    public TypedStudentProfileClient StudentProfiles { get; set; }
    //public StudyCourseClient StudyCourses { get; set; }
    //public StudyGroupClient StudyGroups { get; set; }
    //public SubjectClient Subjects { get; set; }
    public TypedIwentysUserProfileClient IwentysUserProfiles { get; set; }

    public TypedIwentysEntityManagerApiClient(IwentysEntityManagerApiClient client, IMapper mapper)
    {
        StudentProfiles = new TypedStudentProfileClient(client.StudentProfiles, mapper);
        IwentysUserProfiles = new TypedIwentysUserProfileClient(client.IwentysUserProfiles, mapper);
    }
}