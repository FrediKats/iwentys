using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class StudyLeaderboardPage
    {
        //TODO: hack need to implement selection
        private int _courseId = 4;

        private List<StudyCourseInfoDto> _studyCourses;
        private StudyCourseInfoDto _selectedCourse;
        private IReadOnlyList<StudyLeaderboardRowDto> _studentProfiles;

        private string LinkToProfile(StudyLeaderboardRowDto rowDto) => $"student/profile/{rowDto.Student.Id}";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            //_studentProfiles = await ClientHolder.StudyLeaderboard.GetStudyRating(_courseId);
            _studyCourses = await ClientHolder.StudyCourse.Get();
        }

        private async Task OnCurseSelected(StudyCourseInfoDto value)
        {
            _studentProfiles = await ClientHolder.StudyLeaderboard.GetStudyRating(value.CourseId);
        }
    }
}
