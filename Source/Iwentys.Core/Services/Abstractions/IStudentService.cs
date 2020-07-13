using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudentService
    {
        Student[] Get();
        Student Get(int id);
        Student GetOrCreate(int id);

        Student AddGithubUsername(int id, string githubUsername);
        Student RemoveGithubUsername(int id, string githubUsername);
    }
}