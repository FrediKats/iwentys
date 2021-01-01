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
        //TODO: hack need to implement selection
        private int _courseId = 4;

        private IReadOnlyList<StudyLeaderboardRowDto> _studentProfiles;

        private string LinkToProfile(StudyLeaderboardRowDto rowDto) => $"student/profile/{rowDto.Student.Id}";


        protected override async Task OnInitializedAsync()
        {
            var studyLeaderboardControllerClient = new StudyLeaderboardControllerClient(await Http.TrySetHeader(LocalStorage));
            _studentProfiles = await studyLeaderboardControllerClient.GetStudyRating(_courseId);
        }
    }
}
