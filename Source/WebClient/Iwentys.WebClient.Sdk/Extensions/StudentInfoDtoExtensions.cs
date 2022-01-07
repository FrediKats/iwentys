﻿using Iwentys.Sdk;

namespace Iwentys.Endpoints.Sdk.Extensions
{
    public static class StudentInfoDtoExtensions
    {
        public static string GetFullName(this StudentInfoDto userInfo)
        {
            return $"{userInfo.FirstName} {userInfo.SecondName}";
        }
    }
}