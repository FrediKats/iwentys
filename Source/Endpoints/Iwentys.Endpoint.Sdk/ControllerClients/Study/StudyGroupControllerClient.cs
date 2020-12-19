using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class StudyGroupControllerClient
    {
        public StudyGroupControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<GroupProfileResponseDto> Get(string groupName)
        {
            return Client.GetFromJsonAsync<GroupProfileResponseDto>($"api/StudyGroup/by-name/{groupName}");
        }

        public Task<GroupProfileResponseDto> FindStudentGroup(int studentId)
        {
            return Client.GetFromJsonAsync<GroupProfileResponseDto>($"api/StudyGroup/by-student/{studentId}");
        }
    }
}