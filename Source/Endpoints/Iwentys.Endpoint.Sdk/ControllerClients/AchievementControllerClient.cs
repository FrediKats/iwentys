using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Achievements.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class AchievementControllerClient
    {
        public AchievementControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<AchievementDto>> GetForStudent(int studentId)
        {
            return Client.GetFromJsonAsync<List<AchievementDto>>($"/api/achievement/for-student?studentId={studentId}");
        }
    }
}