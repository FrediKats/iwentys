using Iwentys.Core.DomainModel;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTributeService
    {
        Tribute[] GetPendingTributes(AuthorizedUser user);
        Tribute[] GetStudentTributeResult(AuthorizedUser user);
        Tribute CreateTribute(AuthorizedUser user, int projectId);
        Tribute CancelTribute(AuthorizedUser user, int tributeId);
        Tribute CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto);
    }
}