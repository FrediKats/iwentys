using System.Threading.Tasks;
using Iwentys.Features.Study.SubjectAssignments.Enums;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
{
    public partial class SubjectAssignmentSubmitPage
    {
        private SubjectAssignmentSubmitDto _submit;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _submit = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmit(SubjectId, SubmitId);
        }

        private async Task Approve(SubjectAssignmentSubmitDto submit)
        {
            await ClientHolder.SubjectAssignment.SendFeedback(SubjectId, submit.Id, new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = "Smth",
                FeedbackType = FeedbackType.Approve
            });

            _submit = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmit(SubjectId, SubmitId);
        }

        private async Task Reject(SubjectAssignmentSubmitDto submit)
        {
            await ClientHolder.SubjectAssignment.SendFeedback(SubjectId, submit.Id, new SubjectAssignmentSubmitFeedbackArguments
            {
                Comment = "Smth",
                FeedbackType = FeedbackType.Reject
            });

            _submit = await ClientHolder.SubjectAssignment.GetSubjectAssignmentSubmit(SubjectId, SubmitId);
        }
    }
}
