using System;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories.Abstractions;

namespace Iwentys.Core.Daemons
{
    class GithubUpdateDaemon : DaemonWorker
    {
        private readonly IGithubUserDataService _githubUserDataService;
        private readonly IStudentRepository _studentRepository;
        public GithubUpdateDaemon(TimeSpan checkInterval, IGithubUserDataService githubUserDataService, IStudentRepository studentRepository) : base(checkInterval)
        {
            _githubUserDataService = githubUserDataService;
            _studentRepository = studentRepository;
        }

        public override void Execute()
        {
            foreach (var student in _studentRepository.Read().Where(s => s.GithubUsername != null))
            {
                _githubUserDataService.CreateOrUpdate(student.Id);
            }
        }
    }
}
