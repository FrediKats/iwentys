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

        public Task<SubjectProfileDto> GetProfile(int subjectId)
        {
            return Client.GetFromJsonAsync<SubjectProfileDto>($"api/subject/profile/{subjectId}");
        }

        public Task<List<SubjectProfileDto>> GetGroupSubjects(int groupId)
        {
            return Client.GetFromJsonAsync<List<SubjectProfileDto>>($"api/subject/search/for-group?groupId={groupId}");
        }
    }
}