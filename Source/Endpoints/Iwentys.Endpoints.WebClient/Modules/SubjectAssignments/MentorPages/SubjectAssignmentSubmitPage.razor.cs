using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentSubmitPage
    {
        public class Arguments
        {
            public string Comment { get; set; }
            public FeedbackType FeedbackType { get; set; }
            public int? Points { get; set; }
        }

        private SubjectAssignmentSubmitDto _submit;
        private Arguments _arguments = new Arguments();

        protected override async Task OnInitializedAsync()
        {
            _submit = await _subjectAssignmentSubmitClient.GetByIdAsync(SubmitId);
        }


        private async Task Create()
        {
            await _subjectAssignmentSubmitClient.SendSubmitFeedbackAsync(CreateArg(_arguments));
            _navigationManagerClient.NavigateTo($"/subject/{SubjectId}/management/assignments/submits/{SubmitId}", true);
        }

        private SubjectAssignmentSubmitFeedbackArguments CreateArg(Arguments arguments)
        {
            return new SubjectAssignmentSubmitFeedbackArguments
            {
                SubjectAssignmentSubmitId = SubmitId,
                Comment = arguments.Comment,
                FeedbackType = arguments.FeedbackType,
                Points = arguments.Points
            };
        }
    }
}
