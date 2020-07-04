using System;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;

namespace Iwentys.Core.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public Student[] Get()
        {
            return _studentRepository.Read().ToArray();
        }

        public Student Get(int id)
        {
            return _studentRepository.Get(id);
        }

        public Student GetOrCreate(int id)
        {
            throw new NotImplementedException();
        }

        public Student AddGithubUsername(int id, string githubUsername)
        {
            throw new NotImplementedException("Need to validate github credentials");
        }

        public Student RemoveGithubUsername(int id, string githubUsername)
        {
            Student user = _studentRepository.Get(id);
            user.GithubUsername = null;
            return _studentRepository.Update(user);
        }
    }
}