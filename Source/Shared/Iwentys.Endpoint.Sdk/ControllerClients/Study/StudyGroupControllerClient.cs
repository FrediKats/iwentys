using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl.Http;
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

        public Task MakeGroupAdmin(int newGroupAdminId)
        {
            return new FlurlClient(Client)
                .Request("api/StudyGroup/promote-admin", newGroupAdminId)
                .GetAsync();
        }

        public async Task<List<GroupProfileResponseDto>> GetCourseGroups(int courseId)
        {
            return await new FlurlClient(Client)
                .Request("api/StudyGroup")
                .SetQueryParam("courseId", courseId)
                .GetJsonAsync<List<GroupProfileResponseDto>>();
        }
    }
}