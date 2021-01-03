using System.Linq;
using System.Threading.Tasks;
using Iwentys.Features.Raids.Models;
using Iwentys.Features.Study.Models.Students;

namespace Iwentys.Endpoint.Client.Pages.Raids
{
    public partial class RaidProfilePage
    {
        private RaidProfileDto _raid;
        private StudentInfoDto _self;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _raid = await ClientHolder.Raid.Get(RaidId);
            _self = await ClientHolder.Student.GetSelf();
        }

        private async Task RegisterOnRaid()
        {
            await ClientHolder.Raid.RegisterOnRaid(_raid.Id);
            _raid = await ClientHolder.Raid.Get(RaidId);
        }

        private async Task UnRegisterOnRaid()
        {
            await ClientHolder.Raid.UnRegisterOnRaid(_raid.Id);
            _raid = await ClientHolder.Raid.Get(RaidId);
        }

        private bool CanRegisterOnRaid()
        {
            return _self is not null
                   && _raid is not null
                   && _raid.Visitors.All(v => v.Id != _self.Id);
        }

        private bool CanUnRegisterOnRaid()
        {
            return _self is not null
                   && _raid is not null
                   && _raid.Visitors.Any(v => v.Id == _self.Id);
        }
    }
}
