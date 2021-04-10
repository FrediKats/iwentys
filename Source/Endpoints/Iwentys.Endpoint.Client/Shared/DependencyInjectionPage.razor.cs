using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Sdk;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Shared
{
    public partial class DependencyInjectionPage
    {
        //public HttpClient Http => _httpClient;
        public ILocalStorageService LocalStorage => _localStorage;
        public NavigationManager NavigationManager => _navigationManagerClient;

        public AchievementClient AchievementClient { get; set; }
        public AssignmentClient AssignmentClient { get; set; }
        public CompanyClient CompanyClient { get; set; }
        public DebugCommandClient DebugCommandClient { get; set; }
        public GithubClient GithubClient { get; set; }
        public GuildClient GuildClient { get; set; }
        public GuildMembershipClient GuildMembershipClient { get; set; }
        public GuildRecruitmentClient GuildRecruitmentClient { get; set; }
        public GuildTestTaskServiceClient GuildTestTaskServiceClient { get; set; }
        public GuildTributeClient GuildTributeClient { get; set; }
        public InterestTagClient InterestTagClient { get; set; }
        public IsuAuthClient IsuAuthClient { get; set; }
        public KarmaClient KarmaClient { get; set; }
        public LeaderboardClient LeaderboardClient { get; set; }
        public NewsfeedClient NewsfeedClient { get; set; }
        public PeerReviewClient PeerReviewClient { get; set; }
        public QuestClient QuestClient { get; set; }
        public RaidClient RaidClient { get; set; }
        public ScheduleClient ScheduleClient { get; set; }
        public StudentClient StudentClient { get; set; }
        public StudyCourseClient StudyCourseClient { get; set; }
        public StudyGroupClient StudyGroupClient { get; set; }
        public SubjectClient SubjectClient { get; set; }
        public SubjectAssignmentClient SubjectAssignmentClient { get; set; }
        public SubjectAssignmentSubmitClient SubjectAssignmentSubmitClient { get; set; }
        public TournamentClient TournamentClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await _httpClient.TrySetHeader(_localStorage);
            AchievementClient = new AchievementClient(httpClient);
            AssignmentClient = new AssignmentClient(httpClient);
            CompanyClient = new CompanyClient(httpClient);
            DebugCommandClient = new DebugCommandClient(httpClient);
            GithubClient = new GithubClient(httpClient);
            GuildClient = new GuildClient(httpClient);
            GuildMembershipClient = new GuildMembershipClient(httpClient);
            GuildRecruitmentClient = new GuildRecruitmentClient(httpClient);
            GuildTestTaskServiceClient = new GuildTestTaskServiceClient(httpClient);
            GuildTributeClient = new GuildTributeClient(httpClient);
            InterestTagClient = new InterestTagClient(httpClient);
            IsuAuthClient = new IsuAuthClient(httpClient);
            KarmaClient = new KarmaClient(httpClient);
            LeaderboardClient = new LeaderboardClient(httpClient);
            NewsfeedClient = new NewsfeedClient(httpClient);
            PeerReviewClient = new PeerReviewClient(httpClient);
            QuestClient = new QuestClient(httpClient);
            RaidClient = new RaidClient(httpClient);
            ScheduleClient = new ScheduleClient(httpClient);
            StudentClient = new StudentClient(httpClient);
            StudyCourseClient = new StudyCourseClient(httpClient);
            StudyGroupClient = new StudyGroupClient(httpClient);
            SubjectClient = new SubjectClient(httpClient);
            SubjectAssignmentClient = new SubjectAssignmentClient(httpClient);
            SubjectAssignmentSubmitClient = new SubjectAssignmentSubmitClient(httpClient);
            TournamentClient = new TournamentClient(httpClient);
        }
    }
}
