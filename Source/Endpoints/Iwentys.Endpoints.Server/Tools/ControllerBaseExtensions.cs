using System.Security.Claims;
using Iwentys.Features.StudentFeature;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoints.OldServer.Tools
{
    public static class ControllerBaseExtensions
    {
        public static AuthorizedUser TryAuthWithToken(this ControllerBase controller)
        {
            //TODO: for test propose
            const int defaultUserId = 289140;

            ClaimsPrincipal user = controller.HttpContext.User;
            if (user is null)
                return AuthorizedUser.DebugAuth(defaultUserId);

            Claim userIdClaim = user.FindFirst(ClaimTypes.UserData);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return AuthorizedUser.DebugAuth(defaultUserId);

            return AuthorizedUser.DebugAuth(userId);
        }
    }
}