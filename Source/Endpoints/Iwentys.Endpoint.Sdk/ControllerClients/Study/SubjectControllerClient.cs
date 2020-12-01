using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.StudentFeature.ViewModels;

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
    }
}