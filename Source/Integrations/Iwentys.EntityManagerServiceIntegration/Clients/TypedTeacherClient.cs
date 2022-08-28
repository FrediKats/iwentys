using Iwentys.EntityManager.ApiClient;

namespace Iwentys.EntityManagerServiceIntegration;

public class TypedTeacherClient
{
    public TeacherClient Client { get; set; }

    public TypedTeacherClient(TeacherClient client)
    {
        Client = client;
    }

    public async Task<bool> HasTeacherPermissionAsync(int? userId, int? subjectId)
    {
        return await Client.IsUserHasTeacherPermissionForSubjectAsync(userId, subjectId);
    }
}