using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<SubjectProfileDto> _subjects;
        private SubjectProfileDto _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _assignmentControllerClient = new AssignmentControllerClient(httpClient);
            _subjectControllerClient = new SubjectControllerClient(httpClient);
            
            var studyGroupControllerClient = new StudyGroupControllerClient(httpClient);
            var studentControllerClient = new StudentControllerClient(httpClient);
            
            StudentInfoDto student = await studentControllerClient.GetSelf();
            GroupProfileResponseDto studentGroup = await studyGroupControllerClient.FindStudentGroup(student.Id);
            if (studentGroup is not null)
            {
                List<SubjectProfileDto> subject = await _subjectControllerClient.GetGroupSubjects(studentGroup.Id);
                _subjects = new List<SubjectProfileDto>().Append(null).Concat(subject).ToList();
            }
        }

        private async Task ExecuteAssignmentCreation()
        {
            var createArguments = new AssignmentCreateRequestDto(_title, _description, _selectedSubject?.Id, _deadline);
            await _assignmentControllerClient.Create(createArguments);
            NavigationManagerClient.NavigateTo("/assignment");
        }
    }
}
