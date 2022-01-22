using Iwentys.EntityManager.ApiClient;
using System.Net.Http;

namespace Iwentys.WebService.Application;

public class IwentysEntityManagerApiClient
{
    public GroupSubjectClient GroupSubjects { get; set; }
    public StudentProfileClient StudentProfiles { get; set; }
    public StudyCourseClient StudyCourses { get; set; }
    public StudyGroupClient StudyGroups { get; set; }
    public SubjectClient Subjects { get; set; }

    public IwentysEntityManagerApiClient(HttpClient httpClient)
    {
        GroupSubjects = new GroupSubjectClient(httpClient);
        StudentProfiles = new StudentProfileClient(httpClient);
        StudyCourses = new StudyCourseClient(httpClient);
        StudyGroups = new StudyGroupClient(httpClient);
        Subjects = new SubjectClient(httpClient);
    }
}