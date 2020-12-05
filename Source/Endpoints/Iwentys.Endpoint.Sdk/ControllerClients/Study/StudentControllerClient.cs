using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Students.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class StudentControllerClient
    {
        public StudentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<StudentPartialProfileDto>> Get()
        {
            return Client.GetFromJsonAsync<List<StudentPartialProfileDto>>("api/student/profile");
        }

        public Task<StudentPartialProfileDto> GetSelf()
        {
            return Client.GetFromJsonAsync<StudentPartialProfileDto>("api/student/self/");
        }

        public Task<StudentPartialProfileDto> Get(int id)
        {
            return Client.GetFromJsonAsync<StudentPartialProfileDto>($"api/student/profile/{id}");
        }

        public async Task<StudentPartialProfileDto> Update(StudentUpdateRequest studentUpdateRequest)
        {
            HttpResponseMessage responseMessage = await Client.PutAsJsonAsync($"api/student", studentUpdateRequest);
            return await responseMessage.Content.ReadFromJsonAsync<StudentPartialProfileDto>();
        }
    }
}