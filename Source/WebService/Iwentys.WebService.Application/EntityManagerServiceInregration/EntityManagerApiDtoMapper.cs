using Iwentys.Domain.AccountManagement;
using Iwentys.EntityManager.ApiClient;

namespace Iwentys.WebService.Application;

public class EntityManagerApiDtoMapper
{
    public static IwentysUser Map(IwentysUserInfoDto dto)
    {
        return new IwentysUser()
        {
            Id = dto.Id,
            AvatarUrl = dto.AvatarUrl,
            CreationTime = dto.CreationTime,
            FirstName = dto.FirstName,
            GithubUsername = dto.GithubUsername,
            IsAdmin = dto.IsAdmin,
            LastOnlineTime = dto.LastOnlineTime,
            MiddleName = dto.MiddleName,
            SecondName = dto.SecondName,
            BarsPoints = 0
        };
    }
}