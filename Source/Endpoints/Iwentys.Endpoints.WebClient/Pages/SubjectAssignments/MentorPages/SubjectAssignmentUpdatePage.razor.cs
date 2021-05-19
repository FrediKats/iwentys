using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentUpdatePage
    {
        public class Arguments
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public System.DateTime DeadlineUtc { get; set; }
            public int Position { get; set; }
            public bool AvailableForStudent { get; set; }
        }

        private Arguments _arguments = new Arguments();

        private async Task Create()
        {
            await _subjectAssignmentClient.UpdateAsync(CreateArg(_arguments));
            _navigationManagerClient.NavigateTo("/subject/assignment-management");
        }

        private SubjectAssignmentUpdateArguments CreateArg(Arguments arguments)
        {
            return new SubjectAssignmentUpdateArguments
            {
                SubjectAssignmentId = SubjectAssignmentId,
                Title = arguments.Title,
                Description = arguments.Description,
                Link = arguments.Link,
                DeadlineUtc = arguments.DeadlineUtc,
                Position = arguments.Position,
                AvailableForStudent = arguments.AvailableForStudent
            };
        }
    }
}
