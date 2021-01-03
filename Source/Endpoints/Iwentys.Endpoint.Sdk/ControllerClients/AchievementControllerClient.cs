using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
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
            return new FlurlClient(Client)
                .Request($"/api/achievements/students/{studentId}")
                .GetJsonAsync<List<AchievementDto>>();
        }

        public Task<List<AchievementDto>> GetForGuild(int guildId)
        {
            return new FlurlClient(Client)
                .Request($"/api/achievements/guilds/{guildId}")
                .GetJsonAsync<List<AchievementDto>>();
        }
    }
}