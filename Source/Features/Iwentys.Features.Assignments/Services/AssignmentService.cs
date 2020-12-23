using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Common.Tools;
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

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<StudyGroupEntity> _studentGroupRepository;
        private readonly IGenericRepository<AssignmentEntity> _assignmentRepository;
        private readonly IGenericRepository<StudentAssignmentEntity> _studentAssignmentRepository;

        public AssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _assignmentRepository = _unitOfWork.GetRepository<AssignmentEntity>();
            _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignmentEntity>();
        }

        public async Task<AssignmentInfoDto> CreateAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var creator = await _studentRepository.GetByIdAsync(user.Id);
            var studentAssignmentEntity = StudentAssignmentEntity.Create(creator, assignmentCreateRequestDto);

            await _studentAssignmentRepository.InsertAsync(studentAssignmentEntity);
            await _unitOfWork.CommitAsync();
            
            return new AssignmentInfoDto(studentAssignmentEntity);
        }

        public async Task CreateForGroupAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            var creator = await _studentRepository.GetByIdAsync(user.Id);
            var groupAdmin = creator.EnsureIsGroupAdmin();
            var studyGroupEntity = await _studentGroupRepository.GetByIdAsync(groupAdmin.Student.GroupId);

            List<StudentAssignmentEntity> studentAssignmentEntities = StudentAssignmentEntity.CreateForGroup(groupAdmin, assignmentCreateRequestDto, studyGroupEntity);
            foreach (var assignmentEntity in studentAssignmentEntities)
            {
                await _studentAssignmentRepository.InsertAsync(assignmentEntity);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<List<AssignmentInfoDto>> ReadByUserAsync(AuthorizedUser user)
        {
            List<StudentAssignmentEntity> studentAssignmentEntities = await _studentAssignmentRepository
                .GetAsync()
                .Where(a => a.StudentId == user.Id)
                .ToListAsync();

            return studentAssignmentEntities.SelectToList(a => new AssignmentInfoDto(a));
        }

        public async Task<AssignmentInfoDto> ReadByIdAsync(int assignmentId)
        {
            var studentAssignmentEntity = await _studentAssignmentRepository.GetByIdAsync(assignmentId);
            return new AssignmentInfoDto(studentAssignmentEntity);
        }

        public async Task CompleteAsync(AuthorizedUser user, int assignmentId)
        {
            var student = await _studentRepository.GetByIdAsync(user.Id);
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            
            assignment.MarkCompleted(student);
            
            _assignmentRepository.Update(assignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(AuthorizedUser user, int assignmentId)
        {
            var student = await _studentRepository.GetByIdAsync(user.Id);
            var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
            if (student.Id != assignment.CreatorId)
                throw InnerLogicException.Assignment.IsNotAssignmentCreator(assignment.Id, student.Id);

            _assignmentRepository.Delete(assignment);
            await _unitOfWork.CommitAsync();
        }
    }
}