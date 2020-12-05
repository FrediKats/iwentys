using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class SubjectControllerClient
    {
        public SubjectControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<SubjectProfileResponse> GetProfile(int subjectId)
        {
            return Client.GetFromJsonAsync<SubjectProfileResponse>($"api/subject/profile/{subjectId}");
        }

        public Task<List<SubjectProfileResponse>> GetGroupSubjects(int groupId)
        {
            return Client.GetFromJsonAsync<List<SubjectProfileResponse>>($"api/subject/search/for-group?groupId={groupId}");
        }
    }
}