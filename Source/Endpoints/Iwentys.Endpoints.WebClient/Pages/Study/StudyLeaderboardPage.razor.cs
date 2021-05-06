using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Study
{
    public partial class StudyLeaderboardPage
    {
        private List<StudyCourseInfoDto> _studyCourses;
        private List<GroupProfileResponseDto> _groups;

        public StudyCourseInfoDto _selectedCourse;
        private ICollection<StudyLeaderboardRowDto> _studentProfiles;

        private string LinkToProfile(StudyLeaderboardRowDto rowDto) => $"student/profile/{rowDto.Student.Id}";

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _studyCourses = (await StudyCourseClient.StudyCoursesAsync()).ToList();
        }

        private async Task OnCurseSelected(StudyCourseInfoDto value)
        {
            _selectedCourse = value;
            _studentProfiles = await LeaderboardClient.StudyAsync(null, value.CourseId, null, null, null, null);
            _groups = (await StudyGroupClient.GetByCourseIdAsync(value.CourseId)).ToList();
        }

        private async Task OnGroupSelect(GroupProfileResponseDto value)
        {
            _studentProfiles = await LeaderboardClient.StudyAsync(null, _selectedCourse.CourseId, value.Id, null, null, null);
        }
    }
}
