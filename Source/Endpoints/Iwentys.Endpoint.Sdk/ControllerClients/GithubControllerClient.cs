using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients
{
    public class GithubControllerClient
    {
        public GithubControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<CodingActivityInfoResponse>> Get(int studnetId)
        {
            return Client.GetFromJsonAsync<List<CodingActivityInfoResponse>>($"/api/github/student/{studnetId}");
        }

        public Task<List<GithubRepository>> GetStudentRepositories(int studnetId)
        {
            return Client.GetFromJsonAsync<List<GithubRepository>>($"/api/github/student/{studnetId}/repository");
        }
    }
}