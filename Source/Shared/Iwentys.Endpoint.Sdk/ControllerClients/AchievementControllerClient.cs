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

        public Task<List<AchievementInfoDto>> GetForStudent(int studentId)
        {
            return new FlurlClient(Client)
                .Request($"/api/achievements/students/{studentId}")
                .GetJsonAsync<List<AchievementInfoDto>>();
        }

        public Task<List<AchievementInfoDto>> GetForGuild(int guildId)
        {
            return new FlurlClient(Client)
                .Request($"/api/achievements/guilds/{guildId}")
                .GetJsonAsync<List<AchievementInfoDto>>();
        }
    }
}