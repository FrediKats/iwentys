using System.Security.Claims;
using Iwentys.Core.DomainModel;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Api.Tools
{
    public static class ControllerBaseExtensions
    {
        public static AuthorizedUser TryAuthWithToken(this ControllerBase controller)
        {
            ClaimsPrincipal user = controller.HttpContext.User;
            if (user is null)
                return AuthorizedUser.DebugAuth();

            Claim userIdClaim = user.FindFirst(ClaimTypes.UserData);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return AuthorizedUser.DebugAuth();

            return AuthorizedUser.DebugAuth(userId);
        }
    }
}