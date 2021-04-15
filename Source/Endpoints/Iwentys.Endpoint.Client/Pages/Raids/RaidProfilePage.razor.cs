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

            _raid = await RaidClient.GetGetAsync(RaidId);
            _self = await StudentClient.GetSelfAsync();
        }

        private async Task RegisterOnRaid()
        {
            await RaidClient.RegisterAsync(_raid.Id);
            _raid = await RaidClient.GetGetAsync(RaidId);
        }

        private async Task UnRegisterOnRaid()
        {
            await RaidClient.UnregisterAsync(_raid.Id);
            _raid = await RaidClient.GetGetAsync(RaidId);
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
