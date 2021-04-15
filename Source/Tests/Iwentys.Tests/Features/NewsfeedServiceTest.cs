﻿using System.Threading.Tasks;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Models;
using Iwentys.Tests.TestCaseContexts;
using NUnit.Framework;

namespace Iwentys.Tests.Features
{
    [TestFixture]
    public class NewsfeedServiceTest
    {
        [Test]
        public async Task CreateSubjectNews_Ok()
        {
            TestCaseContext testCase = TestCaseContext.Case();
            AuthorizedUser admin = testCase.AccountManagementTestCaseContext.WithUser(true);
            SubjectProfileDto subject = testCase.NewsfeedTestCaseContext.WithSubject();

            NewsfeedViewModel createdNewsfeed = testCase.NewsfeedTestCaseContext.WithSubjectNews(subject, admin);
            NewsfeedViewModel newsfeedFromService = await testCase.NewsfeedService.Get(createdNewsfeed.Id);

            Assert.AreEqual(createdNewsfeed.Content, newsfeedFromService.Content);
        }
    }
}