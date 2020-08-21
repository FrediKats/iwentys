using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTributeService
    {
        TributeInfoDto[] GetPendingTributes(AuthorizedUser user);
        TributeInfoDto[] GetStudentTributeResult(AuthorizedUser user);
        TributeInfoDto CreateTribute(AuthorizedUser user, CreateProjectDto createProject);
        TributeInfoDto CancelTribute(AuthorizedUser user, long tributeId);
        TributeInfoDto CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto);
    }
}