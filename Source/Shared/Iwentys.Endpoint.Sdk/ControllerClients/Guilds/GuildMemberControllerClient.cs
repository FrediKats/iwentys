using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Iwentys.Features.Guilds.Enums;

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

        public async Task<UserMembershipState> GetUserMembership(int guildId)
        {
            return await new FlurlClient(Client)
                .Request($"/api/guild/{guildId}/membership")
                .GetJsonAsync<UserMembershipState>();
        }
    }
}