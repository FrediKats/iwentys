using Iwentys.Features.Study.SubjectAssignments.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class SubjectAssignmentControllerClient
    {
        public SubjectAssignmentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<SubjectAssignmentDto>> GetAssignmentForGroup(int groupId)
        {
            return Client.GetFromJsonAsync<List<SubjectAssignmentDto>>($"api/subject-assignment/for-group/{groupId}");
        }

        public Task<List<SubjectAssignmentDto>> GetAssignmentForGroupSubject(int groupSubjectId)
        {
            return Client.GetFromJsonAsync<List<SubjectAssignmentDto>>($"api/subject-assignment/for-group-subject/{groupSubjectId}");
        }

        public Task<List<SubjectAssignmentDto>> GetSubjectAssignmentForSubject(int subjectId)
        {
            return Client.GetFromJsonAsync<List<SubjectAssignmentDto>>($"api/subject-assignment/for-subject/{subjectId}");
        }
    }
}