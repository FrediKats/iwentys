using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTestTaskService
    {
        GuildTestTaskSolvingInfoEntity Accept(AuthorizedUser user, int guildId);
        GuildTestTaskSolvingInfoEntity Submit(AuthorizedUser user, int guildId, string projectOwner, string projectName);
        GuildTestTaskSolvingInfoEntity Complete(AuthorizedUser user, int guildId, int taskSolveOwnerId);
    }
}