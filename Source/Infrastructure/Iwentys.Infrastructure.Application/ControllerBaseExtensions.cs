using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Iwentys.Infrastructure.Application
{
    public static class ControllerBaseExtensions
    {
        public static AuthorizedUser TryAuthWithToken(this ControllerBase controller)
        {
            return ResolveUserFromIdentity(controller);
        }

        public static AuthorizedUser ResolveUserFromIdentity(this ControllerBase controller)
        {
            Claim findFirst = controller.User.FindFirst("sub");
            if (findFirst is null || !int.TryParse(findFirst.Value, out int userId))
                throw new Exception("User authorize exception");

            return AuthorizedUser.DebugAuth(userId);
        }
    }
}