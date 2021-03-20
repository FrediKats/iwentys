using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Iwentys.Endpoint.Client.Tools;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Shared
{
    public partial class DependencyInjectionPage
    {
        //public HttpClient Http => _httpClient;
        public ILocalStorageService LocalStorage => _localStorage;
        public NavigationManager NavigationManager => _navigationManagerClient;

        public Iwentys.Sdk.AchievementClient AchievementClient { get; set; }
        public Iwentys.Sdk.AssignmentClient AssignmentClient { get; set; }
        public Iwentys.Sdk.CompanyClient CompanyClient { get; set; }
        public Iwentys.Sdk.DebugCommandClient DebugCommandClient { get; set; }
        public Iwentys.Sdk.GithubClient GithubClient { get; set; }
        public Iwentys.Sdk.GuildClient GuildClient { get; set; }
        public Iwentys.Sdk.GuildMembershipClient GuildMembershipClient { get; set; }
        public Iwentys.Sdk.GuildRecruitmentClient GuildRecruitmentClient { get; set; }
        public Iwentys.Sdk.GuildTestTaskServiceClient GuildTestTaskServiceClient { get; set; }
        public Iwentys.Sdk.GuildTributeClient GuildTributeClient { get; set; }
        public Iwentys.Sdk.InterestTagClient InterestTagClient { get; set; }
        public Iwentys.Sdk.IsuAuthClient IsuAuthClient { get; set; }
        public Iwentys.Sdk.KarmaClient KarmaClient { get; set; }
        public Iwentys.Sdk.LeaderboardClient LeaderboardClient { get; set; }
        public Iwentys.Sdk.NewsfeedClient NewsfeedClient { get; set; }
        public Iwentys.Sdk.PeerReviewClient PeerReviewClient { get; set; }
        public Iwentys.Sdk.QuestClient QuestClient { get; set; }
        public Iwentys.Sdk.RaidClient RaidClient { get; set; }
        public Iwentys.Sdk.ScheduleClient ScheduleClient { get; set; }
        public Iwentys.Sdk.StudentClient StudentClient { get; set; }
        public Iwentys.Sdk.StudyCourseClient StudyCourseClient { get; set; }
        public Iwentys.Sdk.StudyGroupClient StudyGroupClient { get; set; }
        public Iwentys.Sdk.SubjectClient SubjectClient { get; set; }
        public Iwentys.Sdk.SubjectAssignmentClient SubjectAssignmentClient { get; set; }
        public Iwentys.Sdk.SubjectAssignmentSubmitClient SubjectAssignmentSubmitClient { get; set; }
        public Iwentys.Sdk.TournamentClient TournamentClient { get; set; }

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await _httpClient.TrySetHeader(_localStorage);
            AchievementClient = new Iwentys.Sdk.AchievementClient(httpClient.BaseAddress.AbsoluteUri, httpClient);
        }
    }
}
