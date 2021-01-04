using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Iwentys.Features.GithubIntegration.Models;
using Iwentys.Features.PeerReview.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class PeerReviewControllerClient
    {
        public PeerReviewControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<List<ProjectReviewRequestInfoDto>> Get()
        {
            return await new FlurlClient(Client)
                .Request("api/peer-review/requests")
                .GetJsonAsync<List<ProjectReviewRequestInfoDto>>();
        }

        public async Task<List<GithubRepositoryInfoDto>> GetAvailableForReviewProject()
        {
            return await new FlurlClient(Client)
                .Request("api/peer-review/requests/available-projects")
                .GetJsonAsync<List<GithubRepositoryInfoDto>>();
        }
    }
}