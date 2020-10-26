using System.Linq;
using Iwentys.Database.Context;
using Iwentys.Models.Entities;
using Iwentys.Models.Transferable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class AssignmentRepository
    {
        private readonly IwentysDbContext _dbContext;

        public AssignmentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public StudentAssignmentEntity Create(StudentEntity creator, AssignmentCreateRequest assignmentCreateRequest)
        {
            EntityEntry<AssignmentEntity> createdAssignment = _dbContext.Assignments.Add(AssignmentEntity.Create(creator, assignmentCreateRequest));
            EntityEntry<StudentAssignmentEntity> studentAssignment = _dbContext.StudentAssignments.Add(new StudentAssignmentEntity
            {
                StudentId = creator.Id,
                Assignment = createdAssignment.Entity
            });

            _dbContext.SaveChanges();
            return studentAssignment.Entity;
        }

        public IQueryable<StudentAssignmentEntity> Read()
        {
            return _dbContext.StudentAssignments
                .Include(sa => sa.Assignment)
                .ThenInclude(a => a.Subject);
        }
    }
}