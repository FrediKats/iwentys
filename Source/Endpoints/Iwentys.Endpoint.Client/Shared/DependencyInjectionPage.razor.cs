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
            AchievementClient = new AchievementClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            AssignmentClient = new AssignmentClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            CompanyClient = new CompanyClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            DebugCommandClient = new DebugCommandClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            GithubClient = new GithubClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            GuildClient = new GuildClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            GuildMembershipClient = new GuildMembershipClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            GuildRecruitmentClient = new GuildRecruitmentClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            GuildTestTaskServiceClient = new GuildTestTaskServiceClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            GuildTributeClient = new GuildTributeClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            InterestTagClient = new InterestTagClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            IsuAuthClient = new IsuAuthClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            KarmaClient = new KarmaClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            LeaderboardClient = new LeaderboardClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            NewsfeedClient = new NewsfeedClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            PeerReviewClient = new PeerReviewClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            QuestClient = new QuestClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            RaidClient = new RaidClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            ScheduleClient = new ScheduleClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            StudentClient = new StudentClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            StudyCourseClient = new StudyCourseClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            StudyGroupClient = new StudyGroupClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            SubjectClient = new SubjectClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            SubjectAssignmentClient = new SubjectAssignmentClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            SubjectAssignmentSubmitClient = new SubjectAssignmentSubmitClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
            TournamentClient = new TournamentClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
        }
    }
}
