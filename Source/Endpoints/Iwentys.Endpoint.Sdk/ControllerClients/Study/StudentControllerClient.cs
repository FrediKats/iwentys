using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class StudentControllerClient
    {
        public StudentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<StudentFullProfileDto>> Get()
        {
            return Client.GetFromJsonAsync<List<StudentFullProfileDto>>("api/student");
        }

        public Task<StudentFullProfileDto> Get(int id)
        {
            return Client.GetFromJsonAsync<StudentFullProfileDto>($"api/student/{id}");
        }

        public Task<List<StudentFullProfileDto>> Get(string groupName)
        {
            return Client.GetFromJsonAsync<List<StudentFullProfileDto>>($"api/student/for-group/{groupName}");
        }

        public async Task<StudentFullProfileDto> Update(StudentUpdateRequest studentUpdateRequest)
        {
            HttpResponseMessage responseMessage = await Client.PutAsJsonAsync($"api/student", studentUpdateRequest);
            return await responseMessage.Content.ReadFromJsonAsync<StudentFullProfileDto>();
        }
    }
}