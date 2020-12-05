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

        public Task<List<StudentInfoDto>> Get()
        {
            return Client.GetFromJsonAsync<List<StudentInfoDto>>("api/student/profile");
        }

        public Task<StudentInfoDto> GetSelf()
        {
            return Client.GetFromJsonAsync<StudentInfoDto>("api/student/self/");
        }

        public Task<StudentInfoDto> Get(int id)
        {
            return Client.GetFromJsonAsync<StudentInfoDto>($"api/student/profile/{id}");
        }

        public async Task<StudentInfoDto> Update(StudentUpdateRequestDto studentUpdateRequestDto)
        {
            HttpResponseMessage responseMessage = await Client.PutAsJsonAsync($"api/student", studentUpdateRequestDto);
            return await responseMessage.Content.ReadFromJsonAsync<StudentInfoDto>();
        }
    }
}