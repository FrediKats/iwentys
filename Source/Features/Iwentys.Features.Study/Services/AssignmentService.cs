using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Models;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class AssignmentService
    {
        private readonly IGenericRepository<Assignment> _assignmentRepository;
        private readonly IGenericRepository<StudentAssignment> _studentAssignmentRepository;
        private readonly IGenericRepository<Student> _studentRepository;

        private readonly IUnitOfWork _unitOfWork;

        public AssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
            _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
        }

        public async Task<List<AssignmentInfoDto>> Create(AuthorizedUser user, AssignmentCreateArguments assignmentCreateArguments)
        {
            Student author = await _studentRepository.GetById(user.Id);

            List<StudentAssignment> assignments = StudentAssignment.Create(author, assignmentCreateArguments);
            
            _studentAssignmentRepository.Insert(assignments);
            await _unitOfWork.CommitAsync();
            return assignments.Select(a => new AssignmentInfoDto(a)).ToList();

        }

        public async Task<List<AssignmentInfoDto>> GetStudentAssignment(AuthorizedUser user)
        {
            return await _studentAssignmentRepository
                .Get()
                .Where(a => a.StudentId == user.Id)
                .Select(AssignmentInfoDto.FromStudentEntity)
                .ToListAsync();
        }

        public async Task<AssignmentInfoDto> GetStudentAssignment(AuthorizedUser user, int assignmentId)
        {
            return await _studentAssignmentRepository
                .Get()
                .Where(sa => sa.AssignmentId == assignmentId && sa.StudentId == user.Id)
                .Select(AssignmentInfoDto.FromStudentEntity)
                .SingleOrDefaultAsync();
        }

        public async Task Complete(AuthorizedUser user, int assignmentId)
        {
            Student student = await _studentRepository.GetById(user.Id);
            Assignment assignment = await _assignmentRepository.GetById(assignmentId);

            StudentAssignment studentAssignment = assignment.MarkCompleted(student);

            _studentAssignmentRepository.Update(studentAssignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task Undo(AuthorizedUser user, int assignmentId)
        {
            Student student = await _studentRepository.GetById(user.Id);
            Assignment assignment = await _assignmentRepository.GetById(assignmentId);

            StudentAssignment studentAssignment = assignment.MarkUncompleted(student);

            _studentAssignmentRepository.Update(studentAssignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task Delete(AuthorizedUser user, int assignmentId)
        {
            Student student = await _studentRepository.GetById(user.Id);
            Assignment assignment = await _assignmentRepository.GetById(assignmentId);

            if (student.Id != assignment.AuthorId)
                throw InnerLogicException.AssignmentExceptions.IsNotAssignmentCreator(assignment.Id, student.Id);

            //FYI: it's coz for dropped cascade. Need to rework after adding cascade deleting
            List<StudentAssignment> studentAssignments = await _studentAssignmentRepository
                .Get()
                .Where(sa => sa.AssignmentId == assignmentId)
                .ToListAsync();

            _studentAssignmentRepository.Delete(studentAssignments);
            _assignmentRepository.Delete(assignment);

            await _unitOfWork.CommitAsync();
        }
    }
}