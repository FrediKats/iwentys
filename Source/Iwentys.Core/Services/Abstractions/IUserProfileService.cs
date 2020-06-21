using Iwentys.Database.Entities;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IUserProfileService
    {
        UserProfile[] Get();
        UserProfile Get(int profileId);
        UserProfile GetOrCreate(int profileId);

        UserProfile AddGithubUsername(int profileId, string githubUsername);
        UserProfile RemoveGithubUsername(int profileId, string githubUsername);
    }
}