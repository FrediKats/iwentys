using System;
using System.Linq;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Repositories;
using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Students;

namespace Iwentys.Core.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public StudentFullProfileDto[] Get()
        {
            return _studentRepository.Read().Select(s => new StudentFullProfileDto(s)).ToArray();
        }

        public StudentFullProfileDto Get(int id)
        {
            return _studentRepository.Get(id).To(s => new StudentFullProfileDto(s));
        }

        public StudentFullProfileDto GetOrCreate(int id)
        {
            throw new NotImplementedException();
        }

        public StudentFullProfileDto AddGithubUsername(int id, string githubUsername)
        {
            throw new NotImplementedException("Need to validate github credentials");
        }

        public StudentFullProfileDto RemoveGithubUsername(int id, string githubUsername)
        {
            Student user = _studentRepository.Get(id);
            user.GithubUsername = null;
            return _studentRepository.Update(user).To(s => new StudentFullProfileDto(s));
        }
    }
}