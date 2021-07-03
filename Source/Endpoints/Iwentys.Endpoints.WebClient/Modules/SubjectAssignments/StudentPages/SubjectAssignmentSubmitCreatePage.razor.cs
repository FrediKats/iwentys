using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.StudentPages
{
    public partial class SubjectAssignmentSubmitCreatePage
    {
        private StudentInfoDto _self;
        private List<SubjectAssignmentJournalItemDto> _subjectAssignments;

        private string _description;
        private SubjectAssignmentJournalItemDto _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _self = await StudentClient.GetSelfAsync();
            _subjectAssignments = (await _studentSubjectAssignmentClient.GetStudentSubjectAssignmentsAsync()).ToList();
        }

        public async Task SendSubmit()
        {
            //TODO: fix
            //var createArguments = new SubjectAssignmentSubmitCreateArguments
            //{
            //    StudentDescription = _description
            //};

            //SubjectAssignmentSubmitDto submit = await SubjectAssignmentSubmitClient.SendSubmitAsync(SubjectId, _selectedSubject.Id, createArguments);

        }
    }
}
