using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.Study.Subjects.Managements
{
    public partial class SubjectAssignmentSubmitPage
    {
        private SubjectAssignmentSubmitDto _submit;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _submit = await SubjectAssignmentSubmitClient.GetByIdAsync(SubjectId, SubmitId);
        }

        private async Task Approve(SubjectAssignmentSubmitDto submit)
        {
            await SubjectAssignmentSubmitClient.SendFeedbackAsync(new SubjectAssignmentSubmitFeedbackArguments
            {
                SubjectAssignmentSubmitId = submit.Id,
                Comment = "Smth",
                FeedbackType = FeedbackType.Approve
            });

            _submit = await SubjectAssignmentSubmitClient.GetByIdAsync(SubjectId, SubmitId);
        }

        private async Task Reject(SubjectAssignmentSubmitDto submit)
        {
            await SubjectAssignmentSubmitClient.SendFeedbackAsync(new SubjectAssignmentSubmitFeedbackArguments
            {
                SubjectAssignmentSubmitId = submit.Id,
                Comment = "Smth",
                FeedbackType = FeedbackType.Reject
            });

            _submit = await SubjectAssignmentSubmitClient.GetByIdAsync(SubjectId, SubmitId);
        }
    }
}
