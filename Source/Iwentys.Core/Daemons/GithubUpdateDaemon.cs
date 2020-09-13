using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;

namespace Iwentys.Core.Daemons
{
    public class GithubUpdateDaemon
    {
        private readonly IGithubUserDataService _githubUserDataService;
        private readonly IStudentRepository _studentRepository;
        public GithubUpdateDaemon(IGithubUserDataService githubUserDataService, IStudentRepository studentRepository)
        {
            _githubUserDataService = githubUserDataService;
            _studentRepository = studentRepository;
        }

        public void Execute()
        {
            foreach (var student in _studentRepository.Read().Where(s => s.GithubUsername != null))
            {
                _githubUserDataService.CreateOrUpdate(student.Id);
            }
        }
    }
}
