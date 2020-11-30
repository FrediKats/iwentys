using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Achievements.ViewModels;
using Iwentys.Models.Transferable;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class AchievementControllerClient
    {
        public AchievementControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<AchievementInfoDto>> GetForStudent(int studentId)
        {
            return Client.GetFromJsonAsync<List<AchievementInfoDto>>($"/api/achievement/for-student?studentId={studentId}");
        }
    }
}