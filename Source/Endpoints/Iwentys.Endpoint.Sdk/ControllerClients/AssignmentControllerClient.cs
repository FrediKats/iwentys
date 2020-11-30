using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.ViewModels;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class AssignmentControllerClient
    {
        public AssignmentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<AssignmentInfoResponse>> Get()
        {
            return Client.GetFromJsonAsync<List<AssignmentInfoResponse>>("/api/assignment");
        }

        public async Task<AssignmentInfoResponse> Create(AssignmentCreateRequest assignmentCreateRequest)
        {
            HttpResponseMessage responseMessage = await Client.PostAsJsonAsync($"api/assignment", assignmentCreateRequest);
            return await responseMessage.Content.ReadFromJsonAsync<AssignmentInfoResponse>();
        }
    }
}