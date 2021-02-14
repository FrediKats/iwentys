using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study
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
            _studyCourses = (await ClientHolder.ApiStudyCoursesAsync()).ToList();
        }

        private async Task OnCurseSelected(StudyCourseInfoDto value)
        {
            _selectedCourse = value;
            _studentProfiles = await ClientHolder.ApiLeaderboardStudyAsync(null, value.CourseId, null, null, null, null);
            _groups = (await ClientHolder.ApiStudygroupAsync(value.CourseId)).ToList();
        }

        private async Task OnGroupSelect(GroupProfileResponseDto value)
        {
            _studentProfiles = await ClientHolder.ApiLeaderboardStudyAsync(null, _selectedCourse.CourseId, value.Id, null, null, null);
        }
    }
}
