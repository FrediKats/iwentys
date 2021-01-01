using System.Net.Http;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Gamification;
using Iwentys.Endpoint.Sdk.ControllerClients.Guilds;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;

namespace Iwentys.Endpoint.Sdk
{
    public class ControllerClientHolder
    {
        public AssignmentControllerClient Assignment { get; }

        public StudentControllerClient Student { get; }

        public SubjectControllerClient Subject{ get; }
        public StudyGroupControllerClient StudyGroup { get; }
        public StudyLeaderboardControllerClient StudyLeaderboard { get; }

        public GuildControllerClient Guild { get; }
        public GuildMemberControllerClient GuildMember { get; }
        public GuildTributeControllerClient GuildTribute { get; }
        public TournamentControllerClient Tournament { get; }

        public NewsfeedControllerClient Newsfeed { get; }

        public GithubControllerClient Github { get; }

        public AchievementControllerClient Achievement { get; }

        public KarmaControllerClient Karma { get; }

        public QuestControllerClient Quest { get; }

        public RaidControllerClient Raid { get; }


        public ControllerClientHolder(HttpClient httpClient)
        {
            Assignment = new AssignmentControllerClient(httpClient);

            Student = new StudentControllerClient(httpClient);

            Subject = new SubjectControllerClient(httpClient);
            StudyGroup = new StudyGroupControllerClient(httpClient);
            StudyLeaderboard = new StudyLeaderboardControllerClient(httpClient);

            Guild = new GuildControllerClient(httpClient);
            GuildMember = new GuildMemberControllerClient(httpClient);
            GuildTribute = new GuildTributeControllerClient(httpClient);
            Tournament = new TournamentControllerClient(httpClient);

            Newsfeed = new NewsfeedControllerClient(httpClient);

            Github = new GithubControllerClient(httpClient);

            Achievement = new AchievementControllerClient(httpClient);

            Karma = new KarmaControllerClient(httpClient);

            Quest = new QuestControllerClient(httpClient);

            Raid = new RaidControllerClient(httpClient);
        }
    }
}