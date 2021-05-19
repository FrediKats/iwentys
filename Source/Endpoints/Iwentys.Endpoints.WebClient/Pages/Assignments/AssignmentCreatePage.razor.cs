using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Assignments
{
    public partial class AssignmentCreatePage
    {
        private string _title;
        private string _description;
        private DateTime? _deadline;
        private bool _forGroup;

        private StudentInfoDto _currentStudent;
        private List<SubjectProfileDto> _subjects;
        private GroupProfileResponseDto _studyGroup;
        private SubjectProfileDto _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _currentStudent = await StudentClient.GetSelfAsync();
            _studyGroup = await StudyGroupClient.GetByStudentIdAsync(_currentStudent.Id);
            if (_studyGroup is not null)
            {
                List<SubjectProfileDto> subject = new List<SubjectProfileDto>();
                subject.AddRange(await SubjectClient.GetSubjectsByGroupIdAsync(_studyGroup.Id));

                //FYI: this value is used in selector
                subject.Insert(0, null);
                _subjects = subject;
            }
        }

        private async Task ExecuteAssignmentCreation()
        {
            var createArguments = new AssignmentCreateArguments
            {
                Title = _title,
                DeadlineTimeUtc = _deadline,
                Description = _description,
                ForStudyGroup = _forGroup,
                Link = null,
                SubjectId = _selectedSubject?.Id
            };
            await AssignmentClient.CreateAsync(createArguments);
            NavigationManager.NavigateTo("/assignment");
        }

        private bool IsUserAdmin()
        {
            return _currentStudent?.Id == _studyGroup?.GroupAdmin?.Id;
        }
    }
}
