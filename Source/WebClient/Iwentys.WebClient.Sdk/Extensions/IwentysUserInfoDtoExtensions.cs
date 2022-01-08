using Iwentys.Sdk;

namespace Iwentys.WebClient.Sdk;

public static class IwentysUserInfoDtoExtensions
{
    public static string GetFullName(this IwentysUserInfoDto userInfo)
    {
        return $"{userInfo.FirstName} {userInfo.SecondName}";
    }
}