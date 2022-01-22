using AutoMapper;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.EntityManagerServiceIntegration;

public class TypedIwentysEntityManagerApiClient
{
    public IwentysEntityManagerApiClient Client { get; }

    //public GroupSubjectClient GroupSubjects { get; set; }
    public TypedStudentProfileClient StudentProfiles { get; set; }
    //public StudyCourseClient StudyCourses { get; set; }
    //public StudyGroupClient StudyGroups { get; set; }
    //public SubjectClient Subjects { get; set; }
    public TypedIwentysUserProfileClient IwentysUserProfiles { get; set; }
    public TypedTeacherClient Teachers { get; set; }

    public TypedIwentysEntityManagerApiClient(IwentysEntityManagerApiClient client, IMapper mapper, HttpClient httpClient)
    {
        Client = client;
        StudentProfiles = new TypedStudentProfileClient(client.StudentProfiles, mapper);
        IwentysUserProfiles = new TypedIwentysUserProfileClient(client.IwentysUserProfiles, mapper);
        Teachers = new TypedTeacherClient(client.Teachers);
    }
}