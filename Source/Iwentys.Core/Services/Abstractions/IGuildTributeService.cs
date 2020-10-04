using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable;
using Iwentys.Models.Transferable.Guilds;
using Iwentys.Models.Transferable.GuildTribute;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IGuildTributeService
    {
        TributeInfoResponse[] GetPendingTributes(AuthorizedUser user);
        TributeInfoResponse[] GetStudentTributeResult(AuthorizedUser user);
        TributeInfoResponse CreateTribute(AuthorizedUser user, CreateProjectRequest createProject);
        TributeInfoResponse CancelTribute(AuthorizedUser user, long tributeId);
        TributeInfoResponse CompleteTribute(AuthorizedUser user, TributeCompleteRequest tributeCompleteRequest);
    }
}