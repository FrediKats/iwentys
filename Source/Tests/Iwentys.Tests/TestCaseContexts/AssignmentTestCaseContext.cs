using Bogus;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Assignments.Models;

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
            var faker = new Faker();
            var createArguments = new AssignmentCreateArguments
            {
                Title = faker.Lorem.Word(),
                Description = faker.Lorem.Word(),
            };

            return _context.AssignmentService.Create(user, createArguments).Result;
        }
    }
}