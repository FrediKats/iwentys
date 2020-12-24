using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Tests.Tools;

namespace Iwentys.Tests.TestCaseContexts
{
    public partial class TestCaseContext
    {
        public TestCaseContext WithInterestTag(out InterestTagDto tag)
        {
            var tagEntity = new InterestTagEntity
            {
                Title = RandomProvider.Faker.Lorem.Word(),
            };
            tagEntity = UnitOfWork.GetRepository<InterestTagEntity>().InsertAsync(tagEntity).Result;
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