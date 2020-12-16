using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class AssignmentControllerClient
    {
        public AssignmentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<AssignmentInfoDto>> Get()
        {
            return Client.GetFromJsonAsync<List<AssignmentInfoDto>>("/api/assignments");
        }

        public async Task<AssignmentInfoDto> Create(AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            HttpResponseMessage responseMessage = await Client.PostAsJsonAsync($"api/assignments", assignmentCreateRequestDto);
            return await responseMessage.Content.ReadFromJsonAsync<AssignmentInfoDto>();
        }

        public Task Complete(int assignmentId)
        {
            return Client.GetAsync($"api/assignments/{assignmentId}/complete");
        }

        public Task Delete(int assignmentId)
        {
            return Client.GetAsync($"api/assignments/{assignmentId}/delete");
        }
    }
}