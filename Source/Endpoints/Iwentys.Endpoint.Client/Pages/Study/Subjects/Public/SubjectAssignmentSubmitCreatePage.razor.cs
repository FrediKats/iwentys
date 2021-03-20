using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Public
{
    public partial class SubjectAssignmentSubmitCreatePage
    {
        private StudentInfoDto _self;
        private List<SubjectAssignmentDto> _subjectAssignments;

        private string _description;
        private SubjectAssignmentDto _selectedSubject;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            _self = await StudentClient.GetSelfAsync();
            _subjectAssignments = (await SubjectAssignmentClient.GetBySubjectIdAsync(SubjectId)).ToList();
        }

        public async Task SendSubmit()
        {
            var createArguments = new SubjectAssignmentSubmitCreateArguments
            {
                StudentDescription = _description
            };

            SubjectAssignmentSubmitDto submit = await SubjectAssignmentSubmitClient.SendSubmitAsync(SubjectId, _selectedSubject.Id, createArguments);

        }
    }
}
