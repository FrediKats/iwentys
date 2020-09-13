using System.Collections.Generic;
using Iwentys.Core.DomainModel;
using Iwentys.Models.Transferable;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IAssignmentService
    {
        AssignmentInfoDto Create(AuthorizedUser user, AssignmentCreateDto assignmentCreateDto);
        List<AssignmentInfoDto> Read(AuthorizedUser user);
    }
}