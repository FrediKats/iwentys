using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Content;

public partial class SubjectAssignmentUpdatePage
{
    public class Arguments
    {
        [Required(AllowEmptyStrings=false,ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }
        [RegularExpression(@"(https?:\/\/)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,4}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)",ErrorMessage = "Url is not valid")]
        public string Link { get; set; }
        public DateTime? DeadlineUtc { get; set; }
        public int Position { get; set; }
        public bool AvailableForStudents { get; set; }
    }

    private Arguments _arguments = new Arguments();

    protected override async Task OnInitializedAsync()
    {
        var assignments = await _mentorSubjectAssignmentClient.GetMentorSubjectAssignmentsAsync();
        var assignment = Enumerable.SelectMany<SubjectAssignmentJournalItemDto, SubjectAssignmentDto>(assignments, s => s.Assignments)
            .First(a => a.Id == SubjectAssignmentId);

        _arguments = new Arguments
        {
            Title = assignment.Title,
            Description = assignment.Description,
            Link = assignment.Link,
            DeadlineUtc = assignment.DeadlineTimeUtc,
            Position = assignment.Position,
            AvailableForStudents = assignment.AvailabilityState == AvailabilityState.Visible
        };
    }

    private async Task Update()
    {
        await _mentorSubjectAssignmentClient.UpdateAsync(CreateArg(_arguments));
        _navigationManager.NavigateTo("/subject/assignment-management/mentor");
    }

    private SubjectAssignmentUpdateArguments CreateArg(Arguments arguments)
    {
        return new SubjectAssignmentUpdateArguments
        {
            SubjectAssignmentId = SubjectAssignmentId,
            Title = arguments.Title,
            Description = arguments.Description,
            Link = arguments.Link,
            DeadlineUtc = arguments.DeadlineUtc.Value,
            Position = arguments.Position,
            AvailabilityState = arguments.AvailableForStudents ? AvailabilityState.Visible : AvailabilityState.Hidden
        };
    }
}