using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Iwentys.Features.Gamification.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Gamification
{
    public class KarmaControllerClient
    {
        public KarmaControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<KarmaStatistic> GetUserKarmaStatistic(int studentId)
        {
            return new FlurlClient(Client)
                .Request("api/karma", studentId)
                .GetJsonAsync<KarmaStatistic>();
        }

        public Task SendUserKarma(int studentId)
        {
            return new FlurlClient(Client)
                .Request("api/karma", studentId)
                .PutAsync();
        }

        public Task RemoveUserKarma(int studentId)
        {
            return new FlurlClient(Client)
                .Request("api/karma", studentId)
                .DeleteAsync();
        }
    }
}