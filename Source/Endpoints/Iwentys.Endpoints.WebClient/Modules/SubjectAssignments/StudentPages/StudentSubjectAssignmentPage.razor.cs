using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Iwentys.Sdk;
using Microsoft.AspNetCore.Components.Authorization;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.StudentPages
{
    public partial class StudentSubjectAssignmentPage
    {
        private enum ViewMode
        {
            All,
            Completed,
            Uncompleted
        }
        
        private ICollection<SubjectAssignmentDto> _subjectAssignments;
        private ICollection<SubjectAssignmentSubmitDto> _subjectAssignmentSubmits;
        private ViewMode _currentViewMode = ViewMode.Uncompleted;
        private StudentInfoDto _self;
        
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _self = await StudentClient.GetSelfAsync();
            _subjectAssignments = await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync(SubjectId);
            _subjectAssignmentSubmits = await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentSubmitsAsync(SubjectId);
        }

        private bool subjectAssignmentSatisfyViewMode(SubjectAssignmentDto subjectAssignment)
        {
            return (_currentViewMode == ViewMode.All
                    || (_currentViewMode == ViewMode.Completed &&
                        subjectAssignment.Submits.Any(
                            submit => submit.Student.Id == _self.Id && (submit.State == SubmitState.Approved))
                        || (_currentViewMode == ViewMode.Uncompleted && !subjectAssignment.Submits.Any(
                            submit => submit.Student.Id == _self.Id && (submit.State == SubmitState.Approved)))
                    ));
        }

        private string LinkToCreateSubmit() => $"/subject/{SubjectId}/assignments/create-submit";
    }
}
