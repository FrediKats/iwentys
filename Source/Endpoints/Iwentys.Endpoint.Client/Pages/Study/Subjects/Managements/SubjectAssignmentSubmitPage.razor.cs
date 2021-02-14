using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Managements
{
    public partial class SubjectAssignmentSubmitPage
    {
        private SubjectAssignmentSubmitDto _submit;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _submit = await ClientHolder.ApiSubjectAssignmentSubmitsGetAsync(SubjectId, SubmitId);
        }

        private async Task Approve(SubjectAssignmentSubmitDto submit)
        {
            await ClientHolder.ApiSubjectAssignmentManagementSubmitsPutAsync(SubjectId, submit.Id, new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = "Smth",
                FeedbackType = FeedbackType.Approve
            });

            _submit = await ClientHolder.ApiSubjectAssignmentSubmitsGetAsync(SubjectId, SubmitId);
        }

        private async Task Reject(SubjectAssignmentSubmitDto submit)
        {
            await ClientHolder.ApiSubjectAssignmentManagementSubmitsPutAsync(SubjectId, submit.Id, new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = "Smth",
                FeedbackType = FeedbackType.Reject
            });

            _submit = await ClientHolder.ApiSubjectAssignmentSubmitsGetAsync(SubjectId, SubmitId);
        }
    }
}
