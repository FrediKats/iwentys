using Iwentys.Sdk;

namespace Iwentys.WebClient.Sdk
{
    public static class StudentInfoDtoExtensions
    {
        public static string GetFullName(this StudentInfoDto userInfo)
        {
            return $"{userInfo.FirstName} {userInfo.SecondName}";
        }
    }
}