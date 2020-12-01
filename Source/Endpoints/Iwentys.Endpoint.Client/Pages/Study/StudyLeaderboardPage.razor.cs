using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Gamification.ViewModels;
using Iwentys.Models.Transferable.Study;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class StudyLeaderboardPage : ComponentBase
    {
        private string LinkToProfile(StudyLeaderboardRow row) => $"student/profile/{row.Student.Id}";

        private IReadOnlyList<StudyLeaderboardRow> _studentProfiles;

        protected override async Task OnInitializedAsync()
        {
            var studyLeaderboardControllerClient = new StudyLeaderboardControllerClient(await Http.TrySetHeader(LocalStorage));
            _studentProfiles = await studyLeaderboardControllerClient.GetStudyRating(CourseId);
        }
    }
}
