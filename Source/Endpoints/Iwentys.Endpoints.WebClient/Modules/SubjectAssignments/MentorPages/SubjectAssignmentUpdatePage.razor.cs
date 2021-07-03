using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentUpdatePage
    {
        public class Arguments
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public DateTime? DeadlineUtc { get; set; }
            public int Position { get; set; }
            public bool AvailableForStudent { get; set; }
        }

        private Arguments _arguments = new Arguments();

        protected override async Task OnInitializedAsync()
        {
            var assignments = await _subjectAssignmentClient.GetAvailableSubjectAssignmentsAsync();
            var assignment = assignments
                .SelectMany(s => s.Assignments)
                .First(a => a.Id == SubjectAssignmentId);

            _arguments = new Arguments
            {
                Title = assignment.Title,
                Description = assignment.Description,
                Link = assignment.Link,
                DeadlineUtc = assignment.DeadlineTimeUtc,
                Position = assignment.Position,
                AvailableForStudent = assignment.AvailableForStudent
            };
        }

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
                //TODO:
                DeadlineUtc = arguments.DeadlineUtc ?? throw new Exception(),
                Position = arguments.Position,
                AvailableForStudent = arguments.AvailableForStudent
            };
        }
    }
}
