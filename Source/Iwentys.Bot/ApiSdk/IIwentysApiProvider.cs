using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Iwentys.ApiClient.OpenAPIService;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public class IwentysApiProvider
    {
        private const string ServiceUrl = "http://localhost:3578";

        //public IGuildApi GuildApi { get; set; }
        //public IQuestApi Quest { get; set; }
        //public IStudentApi Student { get; set; }
        //public IStudyGroupApi StudyGroup { get; set; }
        //public IStudyLeaderboardApi LeaderboardApi { get; set; }
        //public ISubjectApi Subject { get; set; }

        //public IIwentysDebugCommandApi DebugCommand { get; set; }

        public Client Client { get; }

        public IwentysApiProvider()
        {
            var httpClient = new HttpClient();
            Client = new Client(ServiceUrl, httpClient);
        }

        public IwentysApiProvider(string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Client = new Client(ServiceUrl, httpClient);
        }
    }
}