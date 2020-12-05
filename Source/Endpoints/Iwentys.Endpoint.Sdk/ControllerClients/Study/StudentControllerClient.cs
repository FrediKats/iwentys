using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.StudentFeature.Models;

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
            return Client.GetFromJsonAsync<List<StudentFullProfileDto>>("api/student/profile");
        }

        public Task<StudentFullProfileDto> GetSelf()
        {
            return Client.GetFromJsonAsync<StudentFullProfileDto>("api/student/self/");
        }

        public Task<StudentFullProfileDto> Get(int id)
        {
            return Client.GetFromJsonAsync<StudentFullProfileDto>($"api/student/profile/{id}");
        }

        public async Task<StudentFullProfileDto> Update(StudentUpdateRequest studentUpdateRequest)
        {
            HttpResponseMessage responseMessage = await Client.PutAsJsonAsync($"api/student", studentUpdateRequest);
            return await responseMessage.Content.ReadFromJsonAsync<StudentFullProfileDto>();
        }
    }
}