using System.Threading.Tasks;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public class IwentysApiProvider
    {
        private const string ServiceUrl = "http://localhost:3578";

        public IIwentysStudentApi StudentApi { get; set; }
        public IIwentysDebugCommandApi DebugCommand { get; set; }
        public IStudyLeaderboardApi LeaderboardApi { get; set; }
        public IGuildApi GuildApi { get; set; }

        public IwentysApiProvider() : this(new RefitSettings())
        {
            StudentApi = RestService.For<IIwentysStudentApi>(ServiceUrl);
            DebugCommand = RestService.For<IIwentysDebugCommandApi>(ServiceUrl);
            LeaderboardApi = RestService.For<IStudyLeaderboardApi>(ServiceUrl);
            GuildApi = RestService.For<IGuildApi>(ServiceUrl);
        }

        public IwentysApiProvider(RefitSettings settings)
        {
            StudentApi = RestService.For<IIwentysStudentApi>(ServiceUrl, settings);
            DebugCommand = RestService.For<IIwentysDebugCommandApi>(ServiceUrl, settings);
            LeaderboardApi = RestService.For<IStudyLeaderboardApi>(ServiceUrl, settings);
            GuildApi = RestService.For<IGuildApi>(ServiceUrl, settings);
        }

        public static IwentysApiProvider Create(string token)
        {
            var refitSettings = new RefitSettings()
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(token)
            };

            return new IwentysApiProvider(refitSettings);
        }
    }
}