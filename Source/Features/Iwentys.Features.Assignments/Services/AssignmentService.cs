using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Assignments.Services
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

        public async Task<AssignmentInfoDto> Create(AuthorizedUser user, AssignmentCreateArguments assignmentCreateArguments)
        {
            Student author = await _studentRepository.GetById(user.Id);
            StudentAssignment assignment;

            if (assignmentCreateArguments.ForStudyGroup)
            {
                assignment = StudentAssignment.Create(author, assignmentCreateArguments);
                await _studentAssignmentRepository.InsertAsync(assignment);
            }
            else
            {
                GroupAdminUser groupAdmin = author.EnsureIsGroupAdmin();
                List<StudentAssignment> studentAssignmentEntities = StudentAssignment.CreateForGroup(groupAdmin, assignmentCreateArguments);
                await _studentAssignmentRepository.InsertAsync(studentAssignmentEntities);
                assignment = studentAssignmentEntities.First(sa => sa.StudentId == user.Id);
            }

            await _unitOfWork.CommitAsync();
            return new AssignmentInfoDto(assignment);
        }

        public async Task<List<AssignmentInfoDto>> GetUserAssignments(AuthorizedUser user)
        {
            return await _studentAssignmentRepository
                .Get()
                .Where(a => a.StudentId == user.Id)
                .Select(AssignmentInfoDto.FromStudentEntity)
                .ToListAsync();
        }

        public async Task<AssignmentInfoDto> Get(int assignmentId)
        {
            StudentAssignment studentAssignment = await _studentAssignmentRepository.GetById(assignmentId);
            return new AssignmentInfoDto(studentAssignment);
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