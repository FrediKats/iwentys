using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoints.WebClient.Pages.SubjectAssignments.MentorPages
{
    public partial class SubjectAssignmentCreatePage
    {
        private string _title;
        private string _description;

        private async Task Create()
        {
            var arguments = new SubjectAssignmentCreateArguments
            {
                Title = _title,
                Description = _description,
                SubjectId = SubjectId
            };

            await SubjectAssignmentClient.CreateAsync(arguments);
            NavigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments");
        }
    }
}
