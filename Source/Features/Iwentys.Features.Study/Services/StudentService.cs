using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class StudentService
    {
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
        }

        public async Task<List<StudentInfoDto>> Get()
        {
            return await _studentRepository
                .Get()
                .Select(s => new StudentInfoDto(s))
                .ToListAsync();
        }

        public async Task<StudentInfoDto> Get(int id)
        {
            return await _studentRepository
                .GetById(id)
                .To(s => new StudentInfoDto(s));
        }

        public async Task<StudentInfoDto> GetOrCreate(int id)
        {
            Student student = await _studentRepository.FindByIdAsync(id);
            if (student is null)
            {
                var newStudent = Student.CreateFromIsu(id, "userInfo.FirstName", "userInfo.MiddleName", "userInfo.SecondName");
                student = _studentRepository.Insert(newStudent);
            }

            return new StudentInfoDto(student);
        }

        public async Task<StudentInfoDto> Create(StudentCreateArguments createArguments)
        {
            var student = Student.Create(createArguments);

            _studentRepository.Insert(student);
            await _unitOfWork.CommitAsync();

            return new StudentInfoDto(student);
        }
    }
}