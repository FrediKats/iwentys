using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Assignments.Repositories;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Assignments.Services
{
    public class AssignmentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public AssignmentService(IStudentRepository studentRepository, IAssignmentRepository assignmentRepository)
        {
            _studentRepository = studentRepository;
            _assignmentRepository = assignmentRepository;
        }

        public async Task<AssignmentInfoDto> CreateAsync(AuthorizedUser user, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            StudentEntity creator = await user.GetProfile(_studentRepository);
            StudentAssignmentEntity assignment = await _assignmentRepository.CreateAsync(creator, assignmentCreateRequestDto);
            return AssignmentInfoDto.Wrap(assignment);
        }

        public async Task<List<AssignmentInfoDto>> ReadAsync(AuthorizedUser user)
        {
            List<StudentAssignmentEntity> assignments = await _assignmentRepository
                .Read()
                .Include(a => a.Student)
                .Where(a => a.StudentId == user.Id)
                .ToListAsync();

            return assignments.SelectToList(AssignmentInfoDto.Wrap);
        }

        public async Task CompleteAsync(AuthorizedUser user, int assignmentId)
        {
            //TODO: ensure user is creator

            await _assignmentRepository.MarkCompletedAsync(assignmentId);
        }

        public Task DeleteAsync(AuthorizedUser user, int assignmentId)
        {
            //TODO: ensure user is creator
            return _assignmentRepository.DeleteAsync(assignmentId);
        }
    }
}