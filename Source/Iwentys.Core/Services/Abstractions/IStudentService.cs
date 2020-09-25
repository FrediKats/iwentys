using System.Collections.Generic;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Core.Services.Abstractions
{
    public interface IStudentService
    {
        StudentFullProfileDto[] Get();
        StudentFullProfileDto Get(int id);
        List<StudentFullProfileDto> Get(string groupName);
        StudentFullProfileDto GetOrCreate(int id);

        StudentFullProfileDto AddGithubUsername(int id, string githubUsername);
        StudentFullProfileDto RemoveGithubUsername(int id, string githubUsername);
    }
}