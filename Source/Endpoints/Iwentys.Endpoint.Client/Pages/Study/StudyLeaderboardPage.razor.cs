using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Client.Pages.Study
{
    public partial class StudyLeaderboardPage
    {
        private List<StudyCourseInfoDto> _studyCourses;
        private List<GroupProfileResponseDto> _groups;

        public StudyCourseInfoDto _selectedCourse;
        private IReadOnlyList<StudyLeaderboardRowDto> _studentProfiles;

        private string LinkToProfile(StudyLeaderboardRowDto rowDto) => $"student/profile/{rowDto.Student.Id}";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _studyCourses = await ClientHolder.StudyCourse.Get();
        }

        private async Task OnCurseSelected(StudyCourseInfoDto value)
        {
            _selectedCourse = value;
            _studentProfiles = await ClientHolder.StudyLeaderboard.GetStudyRating(value.CourseId);
            _groups = await ClientHolder.StudyGroup.GetCourseGroups(value.CourseId);
        }

        private async Task OnGroupSelect(GroupProfileResponseDto value)
        {
            _studentProfiles = await ClientHolder.StudyLeaderboard.GetStudyRating(_selectedCourse.CourseId, value.Id);
        }
    }
}
