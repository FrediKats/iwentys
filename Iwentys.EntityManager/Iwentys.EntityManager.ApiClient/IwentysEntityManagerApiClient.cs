namespace Iwentys.EntityManager.ApiClient;

public class IwentysEntityManagerApiClient
{
    public GroupSubjectClient GroupSubjects { get; set; }
    public StudentProfileClient StudentProfiles { get; set; }
    public StudyCourseClient StudyCourses { get; set; }
    public StudyGroupClient StudyGroups { get; set; }
    public GroupSubjectClient Subjects { get; set; }
    public IwentysUserProfileClient IwentysUserProfiles { get; set; }

    public IwentysEntityManagerApiClient(HttpClient httpClient)
    {
        GroupSubjects = new GroupSubjectClient(httpClient);
        StudentProfiles = new StudentProfileClient(httpClient);
        StudyCourses = new StudyCourseClient(httpClient);
        StudyGroups = new StudyGroupClient(httpClient);
        Subjects = new GroupSubjectClient(httpClient);
        IwentysUserProfiles = new IwentysUserProfileClient(httpClient);
    }
}