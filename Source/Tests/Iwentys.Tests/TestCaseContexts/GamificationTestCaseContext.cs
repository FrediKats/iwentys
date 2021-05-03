using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.InterestTags;
using Iwentys.Domain.InterestTags.Dto;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public class GamificationTestCaseContext
    {
        private readonly TestCaseContext _context;

        public GamificationTestCaseContext(TestCaseContext context)
        {
            _context = context;
        }

        public InterestTagDto WithInterestTag()
        {
            var tagEntity = new InterestTag
            {
                Title = RandomProvider.Faker.Lorem.Word()
            };

            tagEntity = _context.UnitOfWork.GetRepository<InterestTag>().Insert(tagEntity);
            _context.UnitOfWork.CommitAsync().Wait();

            return new InterestTagDto(tagEntity);
        }
    }
}