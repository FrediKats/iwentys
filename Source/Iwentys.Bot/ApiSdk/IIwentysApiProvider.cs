using System.Threading.Tasks;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public class IwentysApiProvider
    {
        private const string ServiceUrl = "http://localhost:3578";

        public IGuildApi GuildApi { get; set; }
        public IQuestApi Quest { get; set; }
        public IStudentApi Student { get; set; }
        public IStudyGroupApi StudyGroup { get; set; }
        public IStudyLeaderboardApi LeaderboardApi { get; set; }
        public ISubjectApi Subject { get; set; }

        public IIwentysDebugCommandApi DebugCommand { get; set; }

        public IwentysApiProvider() : this(new RefitSettings())
        {
        }

        public IwentysApiProvider(RefitSettings settings)
        {
            GuildApi = RestService.For<IGuildApi>(ServiceUrl, settings);
            Quest = RestService.For<IQuestApi>(ServiceUrl, settings);
            Student = RestService.For<IStudentApi>(ServiceUrl, settings);
            StudyGroup = RestService.For<IStudyGroupApi>(ServiceUrl, settings);
            LeaderboardApi = RestService.For<IStudyLeaderboardApi>(ServiceUrl, settings);
            Subject = RestService.For<ISubjectApi>(ServiceUrl, settings);
            DebugCommand = RestService.For<IIwentysDebugCommandApi>(ServiceUrl, settings);
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