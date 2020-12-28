using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Gamification.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class StudyLeaderboardPage : ComponentBase
    {
        private string LinkToProfile(StudyLeaderboardRowDto rowDto) => $"student/profile/{rowDto.Student.Id}";

        private IReadOnlyList<StudyLeaderboardRowDto> _studentProfiles;

        protected override async Task OnInitializedAsync()
        {
            var studyLeaderboardControllerClient = new StudyLeaderboardControllerClient(await Http.TrySetHeader(LocalStorage));
            //TODO: hack
            _studentProfiles = await studyLeaderboardControllerClient.GetStudyRating(CourseId ?? 4);
        }
    }
}
