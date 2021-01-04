using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Study.Models;
using Iwentys.Features.Study.Models.Students;

namespace Iwentys.Endpoint.Client.Pages.Assignments
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

            _currentStudent = await ClientHolder.Student.GetSelf();
            _studyGroup = await ClientHolder.StudyGroup.FindStudentGroup(_currentStudent.Id);
            if (_studyGroup is not null)
            {
                List<SubjectProfileDto> subject = await ClientHolder.Subject.GetGroupSubjects(_studyGroup.Id);

                //FYI: this value is used in selector
                subject.Insert(0, null);
            }
        }

        private async Task ExecuteAssignmentCreation()
        {
            var createArguments = new AssignmentCreateArguments(_title, _description, _selectedSubject?.Id, _deadline, _forGroup);
            await ClientHolder.Assignment.Create(createArguments);
            NavigationManager.NavigateTo("/assignment");
        }

        private bool IsUserAdmin()
        {
            return _currentStudent?.Id == _studyGroup?.GroupAdmin?.Id;
        }
    }
}
