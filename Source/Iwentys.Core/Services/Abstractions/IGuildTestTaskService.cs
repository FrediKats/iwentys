using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTestTaskService
    {
        List<GuildTestTaskInfoDto> Get(int guildId);

        GuildTestTaskInfoDto Accept(AuthorizedUser user, int guildId);
        GuildTestTaskInfoDto Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName);
        GuildTestTaskInfoDto Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId);
    }
}