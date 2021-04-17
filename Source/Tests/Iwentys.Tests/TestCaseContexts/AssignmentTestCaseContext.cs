﻿using Iwentys.Database.Seeding.FakerEntities;
using Iwentys.Domain;
using Iwentys.Domain.Models;

namespace Iwentys.Tests.TestCaseContexts
{
    public class AssignmentTestCaseContext
    {
        private readonly TestCaseContext _context;

        public AssignmentTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public AssignmentInfoDto WithAssignment(AuthorizedUser user)
        {
            return _context.AssignmentService.Create(user, AssignmentFaker.Instance.CreateAssignmentCreateArguments()).Result;
        }
    }
}