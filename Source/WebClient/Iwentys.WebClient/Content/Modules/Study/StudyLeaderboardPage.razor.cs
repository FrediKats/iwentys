using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class StudyLeaderboardPage
{
    private List<StudyCourseInfoDto> _studyCourses;
    private List<StudyGroupProfileResponseDto> _groups;

    public StudyCourseInfoDto _selectedCourse;
    private StudyGroupProfileResponseDto _selectedGroup;
    private ICollection<StudyLeaderboardRowDto> _studentProfiles;

    private string LinkToProfile(StudyLeaderboardRowDto rowDto) => $"student/profile/{rowDto.Student.Id}";

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        _studyCourses = (await _studyCourseClient.StudyCoursesAsync()).ToList();
    }

    private async Task OnCurseSelected(StudyCourseInfoDto value)
    {
        _selectedCourse = value;
        _studentProfiles = await _leaderboardClient.StudyAsync(null, value.CourseId, null, null, null, null);
        _groups = (await _studyGroupClient.GetByCourseIdAsync(value.CourseId)).ToList();
    }

    private async Task OnGroupSelect(StudyGroupProfileResponseDto value)
    {
        _studentProfiles = await _leaderboardClient.StudyAsync(null, _selectedCourse.CourseId, value.Id, null, null, null);
    }
}