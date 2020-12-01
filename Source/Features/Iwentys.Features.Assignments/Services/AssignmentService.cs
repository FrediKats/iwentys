using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Repositories;
using Iwentys.Features.Assignments.ViewModels;
using Iwentys.Features.StudentFeature;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;
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

        public async Task<AssignmentInfoResponse> CreateAsync(AuthorizedUser user, AssignmentCreateRequest assignmentCreateRequest)
        {
            StudentEntity creator = await user.GetProfile(_studentRepository);
            StudentAssignmentEntity assignment = await _assignmentRepository.CreateAsync(creator, assignmentCreateRequest);
            return AssignmentInfoResponse.Wrap(assignment);
        }

        public async Task<List<AssignmentInfoResponse>> ReadAsync(AuthorizedUser user)
        {
            List<StudentAssignmentEntity> assignments = await _assignmentRepository
                .Read()
                .Include(a => a.Student)
                .Where(a => a.StudentId == user.Id)
                .ToListAsync();

            return assignments.SelectToList(AssignmentInfoResponse.Wrap);
        }
    }
}