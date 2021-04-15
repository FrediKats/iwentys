namespace Iwentys.Sdk.Extensions
{
    public static class IwentysUserInfoDtoExtensions
    {
        public static string GetFullName(this IwentysUserInfoDto userInfo)
        {
            return $"{userInfo.FirstName} {userInfo.SecondName}";
        }
    }
}