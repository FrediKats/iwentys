using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iwentys.Endpoint.Client.Pages.Guilds.Tributes
{
    public partial class TributeCardComponent
    {
        public void Complete()
        {
            NavigationManagerClient.NavigateTo($"/guild/{Tribute.GuildId}/tribute/{Tribute.Project.Id}/response");
        }
    }
}
