using System.Linq;
using System.Threading.Tasks;
using Iwentys.Sdk;

namespace Iwentys.Endpoint.Client.Pages.Raids
{
    public partial class RaidProfilePage
    {
        private RaidProfileDto _raid;
        private StudentInfoDto _self;
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _raid = await ClientHolder.ApiRaidsProfileGetAsync(RaidId);
            _self = await ClientHolder.ApiStudentSelfAsync();
        }

        private async Task RegisterOnRaid()
        {
            await ClientHolder.ApiRaidsProfileRegisterAsync(_raid.Id);
            _raid = await ClientHolder.ApiRaidsProfileGetAsync(RaidId);
        }

        private async Task UnRegisterOnRaid()
        {
            await ClientHolder.ApiRaidsProfileUnregisterAsync(_raid.Id);
            _raid = await ClientHolder.ApiRaidsProfileGetAsync(RaidId);
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
