using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Domain;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.Models.Students;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudentService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork, AchievementProvider achievementProvider)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
        }

        public async Task<List<StudentInfoDto>> Get()
        {
            List<Student> students = await _studentRepository
                .Get()
                .ToListAsync();

            return students.SelectToList(s => new StudentInfoDto(s));
        }

        public async Task<StudentInfoDto> Get(int id)
        {
            Student student = await _studentRepository.GetById(id);
            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> GetOrCreate(int id)
        {
            Student student = await _studentRepository.FindByIdAsync(id);
            if (student is null)
            {
                var newStudent = Student.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName");
                await _studentRepository.InsertAsync(newStudent);
                student = await _studentRepository.FindByIdAsync(newStudent.Id);
            }

            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> Create(StudentCreateArguments createArguments)
        {
            Student student = await _studentRepository.GetById(id);
            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> RemoveGithubUsername(int id, string githubUsername)
        {
            Student user = await _studentRepository.FindByIdAsync(id);
            user.GithubUsername = githubUsername;
            _studentRepository.Update(user);
            return new StudentInfoDto(await _studentRepository.FindByIdAsync(id));
        }
    }
}