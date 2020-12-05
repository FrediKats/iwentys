using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Iwentys.Endpoint.Client.Tools;
using Iwentys.Endpoint.Sdk.ControllerClients;
using Iwentys.Endpoint.Sdk.ControllerClients.Study;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.StudentFeature.Models;
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

        private List<SubjectProfileResponse> _subjects;
        private SubjectProfileResponse _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            HttpClient httpClient = await Http.TrySetHeader(LocalStorage);
            _assignmentControllerClient = new AssignmentControllerClient(httpClient);
            _subjectControllerClient = new SubjectControllerClient(httpClient);
            
            var studyGroupControllerClient = new StudyGroupControllerClient(httpClient);
            var studentControllerClient = new StudentControllerClient(httpClient);
            
            StudentPartialProfileDto student = await studentControllerClient.GetSelf();
            GroupProfileResponse studentGroup = await studyGroupControllerClient.GetStudentGroup(student.Id);
            if (studentGroup is not null)
            {
                List<SubjectProfileResponse> subject = await _subjectControllerClient.GetGroupSubjects(studentGroup.Id);
                _subjects = new List<SubjectProfileResponse>().Append(null).Concat(subject).ToList();
            }
        }

        private async Task ExecuteAssignmentCreation()
        {
            var createArguments = new AssignmentCreateRequestDto()
            {
                Title = _title,
                Description = _description,
                Deadline = _deadline
            };

            if (_selectedSubject is not null)
            {
                createArguments.SubjectId = _selectedSubject.Id;
            }

            await _assignmentControllerClient.Create(createArguments);
            NavigationManagerClient.NavigateTo("/assignment");
        }
    }
}
