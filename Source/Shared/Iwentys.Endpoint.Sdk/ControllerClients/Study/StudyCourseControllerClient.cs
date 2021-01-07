using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class StudyCourseControllerClient
    {
        public StudyCourseControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public async Task<List<StudyCourseInfoDto>> Get()
        {
            return await new FlurlClient(Client)
                .Request("api/study-courses")
                .GetJsonAsync<List<StudyCourseInfoDto>>();
        }
    }
}