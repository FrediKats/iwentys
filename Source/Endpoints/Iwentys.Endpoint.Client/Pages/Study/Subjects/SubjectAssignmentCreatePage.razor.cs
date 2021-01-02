using System;
using System.Threading.Tasks;
using Iwentys.Features.Study.SubjectAssignments.Models;

namespace Iwentys.Endpoint.Client.Pages.Study.Subjects
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

            await ClientHolder.SubjectAssignment.CreateSubjectAssignment(SubjectId, arguments);
            NavigationManager.NavigateTo($"/subject/{SubjectId}/assignment/manage");
        }
    }
}
