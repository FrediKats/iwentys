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
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<StudyGroup> _studentGroupRepository;
        private readonly IGenericRepository<Assignment> _assignmentRepository;
        private readonly IGenericRepository<StudentAssignment> _studentAssignmentRepository;
        private readonly IGenericRepository<StudyGroup> _studyGroupRepository;

        public AssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = _unitOfWork.GetRepository<Student>();
            _studentGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
            _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
            _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
            _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
        }

        public async Task<AssignmentInfoDto> CreateAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            Student creator = await _studentRepository.FindByIdAsync(user.Id);
            StudentAssignment assignment;

            if (assignmentCreateRequestDto.ForStudyGroup)
            {
                assignment = StudentAssignment.Create(creator, assignmentCreateRequestDto);
                await _studentAssignmentRepository.InsertAsync(assignment);
            }
            else
            {
                StudyGroup studyGroup = creator.Group.Group;
                GroupAdminUser groupAdmin = creator.EnsureIsGroupAdmin();
                List<StudentAssignment> studentAssignmentEntities = StudentAssignment.CreateForGroup(groupAdmin, assignmentCreateRequestDto, studyGroup);
                await _studentAssignmentRepository.InsertAsync(studentAssignmentEntities);
                assignment = studentAssignmentEntities.First(sa => sa.StudentId == user.Id);
            }

            await _unitOfWork.CommitAsync();
            return new AssignmentInfoDto(assignment);
        }

        public async Task CreateForGroupAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            Student creator = await _studentRepository.FindByIdAsync(user.Id);
            GroupAdminUser groupAdmin = creator.EnsureIsGroupAdmin();

            
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
            StudentAssignment studentAssignmentEntity = await _studentAssignmentRepository.FindByIdAsync(assignmentId);
            return new AssignmentInfoDto(studentAssignmentEntity);
        }

        public async Task CompleteAsync(AuthorizedUser user, int assignmentId)
        {
            Student student = await _studentRepository.FindByIdAsync(user.Id);
            Assignment assignment = await _assignmentRepository.FindByIdAsync(assignmentId);

            StudentAssignment studentAssignment = assignment.MarkCompleted(student);

            _studentAssignmentRepository.Update(studentAssignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task UndoAsync(AuthorizedUser user, int assignmentId)
        {
            Student student = await _studentRepository.FindByIdAsync(user.Id);
            Assignment assignment = await _assignmentRepository.FindByIdAsync(assignmentId);

            StudentAssignment studentAssignment = assignment.MarkUncompleted(student);

            _studentAssignmentRepository.Update(studentAssignment);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(AuthorizedUser user, int assignmentId)
        {
            Student student = await _studentRepository.FindByIdAsync(user.Id);
            Assignment assignment = await _assignmentRepository.FindByIdAsync(assignmentId);
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