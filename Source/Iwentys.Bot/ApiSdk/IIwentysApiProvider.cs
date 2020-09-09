using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public class IwentysApiProvider
    {
        public IIwentysStudentApi StudentApi { get; set; }
        public IIwentysDebugCommandApi DebugCommand { get; set; }
        public IStudyLeaderboardApi LeaderboardApi { get; set; }
        public IGuildApi GuildApi { get; set; }

        public IwentysApiProvider(string hostUrl)
        {
            StudentApi = RestService.For<IIwentysStudentApi>(hostUrl);
            DebugCommand = RestService.For<IIwentysDebugCommandApi>(hostUrl);
            LeaderboardApi = RestService.For<IStudyLeaderboardApi>(hostUrl);
            GuildApi = RestService.For<IGuildApi>(hostUrl);
        }
    }
}