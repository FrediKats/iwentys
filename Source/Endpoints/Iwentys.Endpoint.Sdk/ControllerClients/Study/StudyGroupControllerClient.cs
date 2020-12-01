using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.StudentFeature.ViewModels;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class StudyGroupControllerClient
    {
        public StudyGroupControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<GroupProfileResponse> Get(string groupName)
        {
            return Client.GetFromJsonAsync<GroupProfileResponse>($"api/StudyGroup/by-name/{groupName}");
        }
    }
}