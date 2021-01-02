using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithInterestTag(out InterestTagDto tag)
        {
            var tagEntity = new InterestTag
            {
                Title = RandomProvider.Faker.Lorem.Word(),
            };
            tagEntity = UnitOfWork.GetRepository<InterestTag>().InsertAsync(tagEntity).Result;
            UnitOfWork.CommitAsync().Wait();

            tag = new InterestTagDto(tagEntity);
            return this;
        }

        public TestCaseContext WithUserInterestTag(InterestTagDto tag, AuthorizedUser user)
        {
            InterestTagService.AddStudentTag(user.Id, tag.Id).Wait();
            return this;
        }
    }
}