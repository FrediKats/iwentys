using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTributeService
    {
        TributeInfoDto[] GetPendingTributes(AuthorizedUser user);
        TributeInfoDto[] GetStudentTributeResult(AuthorizedUser user);
        TributeInfoDto CreateTribute(AuthorizedUser user, int projectId);
        TributeInfoDto CancelTribute(AuthorizedUser user, int tributeId);
        TributeInfoDto CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto);
    }
}