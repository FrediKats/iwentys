using System;
using System.Security.Claims;
using Iwentys.Endpoints.Api.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application
{
    public static class ControllerBaseExtensions
    {
        public static AuthorizedUser TryAuthWithToken(this ControllerBase controller)
        {
            //FYI: for test propose
            const int defaultUserId = 228617;
            return TryAuthWithTokenOrDefault(controller, defaultUserId);
        }

        public static AuthorizedUser TryAuthWithTokenOrDefault(this ControllerBase controller, int defaultUserId)
        {
            ClaimsPrincipal user = controller.User;
            Claim userIdClaim = user.FindFirst(ClaimTypes.UserData);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
                return AuthorizedUser.DebugAuth(defaultUserId);

            return AuthorizedUser.DebugAuth(userId);
        }

        public static AuthorizedUser TryAuthWithIdentity(this ControllerBase controller, UserManager<ApplicationUser> userManager)
        {
            Claim findFirst = controller.User.FindFirst("sub");
            if (findFirst is null || !int.TryParse(findFirst.Value, out int userId))
                throw new Exception("User authorize exception");

            return AuthorizedUser.DebugAuth(userId);
        }
    }
}