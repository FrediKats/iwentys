using System;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Exceptions;
using Iwentys.Database.Context;
using Iwentys.Features.Assignments.Entities;
using Iwentys.Features.Assignments.Models;
using Iwentys.Features.Assignments.Repositories;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Iwentys.Database.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly IwentysDbContext _dbContext;

        public AssignmentRepository(IwentysDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<StudentAssignmentEntity> CreateAsync(StudentEntity creator, AssignmentCreateRequestDto assignmentCreateRequestDto)
        {
            EntityEntry<AssignmentEntity> createdAssignment = await _dbContext.Assignments.AddAsync(AssignmentEntity.Create(creator, assignmentCreateRequestDto));
            EntityEntry<StudentAssignmentEntity> studentAssignment = await _dbContext.StudentAssignments.AddAsync(new StudentAssignmentEntity
            {
                StudentId = creator.Id,
                Assignment = createdAssignment.Entity
            });

            await _dbContext.SaveChangesAsync();
            return studentAssignment.Entity;
        }

        public IQueryable<StudentAssignmentEntity> Read()
        {
            return _dbContext.StudentAssignments
                .Include(sa => sa.Assignment)
                .ThenInclude(a => a.Subject);
        }

        public async Task<AssignmentEntity> MarkCompletedAsync(int assignmentId)
        {
            AssignmentEntity assignmentEntity = await _dbContext.Assignments.FindAsync(assignmentId);
            if (assignmentEntity is null)
                throw new EntityNotFoundException("Assignment was not found.");
            assignmentEntity.IsCompleted = true;
            EntityEntry<AssignmentEntity> result = _dbContext.Assignments.Update(assignmentEntity);
            await _dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(int assignmentId)
        {
            AssignmentEntity assignment = await _dbContext.Assignments.FindAsync(assignmentId);
            _dbContext.Assignments.Remove(assignment);
            await _dbContext.SaveChangesAsync();
        }
    }
}