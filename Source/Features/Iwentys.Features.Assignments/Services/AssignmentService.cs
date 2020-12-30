using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Assignments.Services
{
    public class AssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<StudyGroup> _studentGroupRepository;
        private readonly IGenericRepository<Assignment> _assignmentRepository;
        private readonly IGenericRepository<StudentAssignment> _studentAssignmentRepository;

        public AssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _studentGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
            _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
            _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
        }

        public async Task<AssignmentInfoDto> CreateAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var creator = await _studentRepository.FindByIdAsync(user.Id);
            var studentAssignmentEntity = StudentAssignment.Create(creator, assignmentCreateRequestDto);

            await _studentAssignmentRepository.InsertAsync(studentAssignmentEntity);
            await _unitOfWork.CommitAsync();
            
            return new AssignmentInfoDto(studentAssignmentEntity);
        }

        public async Task CreateForGroupAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var creator = await _studentRepository.FindByIdAsync(user.Id);
            var groupAdmin = creator.EnsureIsGroupAdmin();
            var studyGroupEntity = await _studentGroupRepository.FindByIdAsync(groupAdmin.Student.GroupId);

            List<StudentAssignment> studentAssignmentEntities = StudentAssignment.CreateForGroup(groupAdmin, assignmentCreateRequestDto, studyGroupEntity);
            foreach (var assignmentEntity in studentAssignmentEntities)
            {
                await _studentAssignmentRepository.InsertAsync(assignmentEntity);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<List<AssignmentInfoDto>> ReadByUserAsync(AuthorizedUser user)
        {
            return await _studentAssignmentRepository
                .Get()
                .Where(a => a.StudentId == user.Id)
                .Select(AssignmentInfoDto.FromStudentEntity)
                .ToListAsync();
        }

        public async Task<AssignmentInfoDto> ReadByIdAsync(int assignmentId)
        {
            var studentAssignmentEntity = await _studentAssignmentRepository.FindByIdAsync(assignmentId);
            return new AssignmentInfoDto(studentAssignmentEntity);
        }

        public async Task CompleteAsync(AuthorizedUser user, int assignmentId)
        {
            var student = await _studentRepository.FindByIdAsync(user.Id);
            var assignment = await _assignmentRepository.FindByIdAsync(assignmentId);
            
            assignment.MarkCompleted(student);
            
            _assignmentRepository.Update(assignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task UndoAsync(AuthorizedUser user, int assignmentId)
        {
            var student = await _studentRepository.FindByIdAsync(user.Id);
            var assignment = await _assignmentRepository.FindByIdAsync(assignmentId);

            assignment.MarkUncompleted(student);

            _assignmentRepository.Update(assignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(AuthorizedUser user, int assignmentId)
        {
            var student = await _studentRepository.FindByIdAsync(user.Id);
            var assignment = await _assignmentRepository.FindByIdAsync(assignmentId);
            if (student.Id != assignment.CreatorId)
                throw InnerLogicException.AssignmentExceptions.IsNotAssignmentCreator(assignment.Id, student.Id);

            //FYI: it's coz for dropped cascade
            List<StudentAssignment> studentAssignmentEntities = await _studentAssignmentRepository
                .Get()
                .Where(sa => sa.AssignmentId == assignmentId)
                .ToListAsync();
            studentAssignmentEntities.ForEach(sa => _studentAssignmentRepository.Delete(sa));
            
            _assignmentRepository.Delete(assignment);
            await _unitOfWork.CommitAsync();
        }
    }
}