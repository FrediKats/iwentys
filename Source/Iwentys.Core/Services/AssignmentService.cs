using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Database.Context;
using Iwentys.Features.StudentFeature;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Core.Services
{
    public class AssignmentService
    {
        private readonly DatabaseAccessor _database;

        public AssignmentService(DatabaseAccessor database)
        {
            _database = database;
        }

        public async Task<AssignmentInfoResponse> CreateAsync(AuthorizedUser user, AssignmentCreateRequest assignmentCreateRequest)
        {
            StudentEntity creator = await user.GetProfile(_database.Student);
            StudentAssignmentEntity assignment = await _database.Assignment.CreateAsync(creator, assignmentCreateRequest);
            return AssignmentInfoResponse.Wrap(assignment);
        }

        public async Task<List<AssignmentInfoResponse>> ReadAsync(AuthorizedUser user)
        {
            List<StudentAssignmentEntity> assignments = await _database.Assignment
                .Read()
                .Where(a => a.StudentId == user.Id)
                .ToListAsync();

            return assignments.SelectToList(AssignmentInfoResponse.Wrap);
        }
    }
}