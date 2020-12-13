﻿using System.Security.Claims;
using Iwentys.Features.Students.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Endpoint.Controllers.Tools
{
    public static class ControllerBaseExtensions
    {
        public static AuthorizedUser TryAuthWithToken(this ControllerBase controller)
        {
            //TODO: for test propose
            const int defaultUserId = 228617;
            return TryAuthWithTokenOrDefault(controller, defaultUserId);
        }

        public static AuthorizedUser TryAuthWithTokenOrDefault(this ControllerBase controller, int defaultUserId)
        {
            ClaimsPrincipal user = controller.HttpContext.User;

            Claim userIdClaim = user.FindFirst(ClaimTypes.UserData);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
                return AuthorizedUser.DebugAuth(defaultUserId);

            return AuthorizedUser.DebugAuth(userId);
        }

    }
}