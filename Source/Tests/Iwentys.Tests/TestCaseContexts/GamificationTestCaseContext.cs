﻿using Iwentys.Domain;
using Iwentys.Domain.Models;
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

            tagEntity = _context.UnitOfWork.GetRepository<InterestTag>().InsertAsync(tagEntity).Result;
            _context.UnitOfWork.CommitAsync().Wait();

            return new InterestTagDto(tagEntity);
        }

        public void WithUserInterestTag(InterestTagDto tag, AuthorizedUser user)
        {
            _context.InterestTagService.AddUserTag(user.Id, tag.Id).Wait();
        }
    }
}