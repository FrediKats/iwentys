using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTestTaskService
    {
        List<GuildTestTaskInfoResponse> Get(int guildId);

        GuildTestTaskInfoResponse Accept(AuthorizedUser user, int guildId);
        GuildTestTaskInfoResponse Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName);
        GuildTestTaskInfoResponse Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId);
    }
}