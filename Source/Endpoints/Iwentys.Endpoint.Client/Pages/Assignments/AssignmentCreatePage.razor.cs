using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Models;
using Iwentys.Features.Study.Models;
using Microsoft.AspNetCore.Components;

namespace Iwentys.Endpoint.Client.Pages.Assignments
{
    public partial class AssignmentCreatePage : ComponentBase
    {
        private AssignmentControllerClient _assignmentControllerClient;
        private SubjectControllerClient _subjectControllerClient;

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
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _assignmentControllerClient = new AssignmentControllerClient(httpClient);
            _subjectControllerClient = new SubjectControllerClient(httpClient);
            
            var studyGroupControllerClient = new StudyGroupControllerClient(httpClient);
            var studentControllerClient = new StudentControllerClient(httpClient);

            _currentStudent = await studentControllerClient.GetSelf();
            _studyGroup = await studyGroupControllerClient.FindStudentGroup(_currentStudent.Id);
            if (_studyGroup is not null)
            {
                List<SubjectProfileDto> subject = await _subjectControllerClient.GetGroupSubjects(_studyGroup.Id);

                //FYI: this value is used in selector
                subject.Insert(0, null);
            }
        }

        private async Task ExecuteAssignmentCreation()
        {
            var createArguments = new AssignmentCreateRequestDto(_title, _description, _selectedSubject?.Id, _deadline, _forGroup);
            await _assignmentControllerClient.Create(createArguments);
            NavigationManagerClient.NavigateTo("/assignment");
        }

        private bool IsUserAdmin()
        {
            return _currentStudent?.Id == _studyGroup?.GroupAdmin?.Id;
        }
    }
}
