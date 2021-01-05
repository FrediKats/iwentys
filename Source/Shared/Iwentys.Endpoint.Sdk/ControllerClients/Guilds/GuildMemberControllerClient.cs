using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Guilds
{
    public class GuildMemberControllerClient
    {
        public GuildMemberControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task Leave(int guildId)
        {
            return new FlurlClient(Client)
                .Request($"/api/guild/{guildId}/leave")
                .PutAsync();
        }

        public async Task KickGuildMember(int guildId, int memberId)
        {
            await new FlurlClient(Client)
                .Request($"/api/guild/{guildId}/member/{memberId}/kick")
                .PutAsync();
        }

        public async Task PromoteToMentor(int guildId, int memberId)
        {
            await new FlurlClient(Client)
                .Request($"/api/guild/{guildId}/member/{memberId}/promote")
                .PutAsync();
        }
    }
}