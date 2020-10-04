using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IAssignmentService
    {
        AssignmentInfoResponse Create(AuthorizedUser user, AssignmentCreateRequest assignmentCreateRequest);
        List<AssignmentInfoResponse> Read(AuthorizedUser user);
    }
}