using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Modules.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentCreatePage
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

        private async Task Create()
        {
            await _mentorSubjectAssignmentClient.CreateAsync(CreateArg(_arguments));
            _navigationManager.NavigateTo("/subject/assignment-management/mentor");
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
