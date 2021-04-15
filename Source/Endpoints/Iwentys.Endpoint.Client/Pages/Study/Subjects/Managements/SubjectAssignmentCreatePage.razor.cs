using System;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects.Managements
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
                Description = _description
            };

            await SubjectAssignmentClient.CreateAsync(SubjectId, arguments);
            NavigationManager.NavigateTo($"/subject/{SubjectId}/management/assignments");
        }
    }
}
