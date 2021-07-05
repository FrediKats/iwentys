using System;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentCreatePage
    {
        public class Arguments
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public DateTime DeadlineUtc { get; set; }
            public int Position { get; set; }
            public bool AvailableForStudents { get; set; }
        }

        private Arguments _arguments = new Arguments();

        private async Task Create()
        {
            await _subjectAssignmentClient.CreateAsync(CreateArg(_arguments));
            _navigationManager.NavigateTo("/subject/assignment-management");
        }

        private SubjectAssignmentCreateArguments CreateArg(Arguments arguments)
        {
            return new SubjectAssignmentCreateArguments
            {
                SubjectId = SubjectId,
                Title = arguments.Title,
                Description = arguments.Description,
                Link = arguments.Link,
                DeadlineUtc = arguments.DeadlineUtc,
                Position = arguments.Position,
                AvailabilityState = arguments.AvailableForStudents ? AvailabilityState.Visible : AvailabilityState.Hidden
            };
        }
    }
}
