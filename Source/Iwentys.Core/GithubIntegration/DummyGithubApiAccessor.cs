﻿using System;
using System.Collections.Generic;
using Iwentys.Models.Types.Github;

namespace Iwentys.Core.GithubIntegration
{
    public class DummyGithubApiAccessor : IGithubApiAccessor
    {
        public GithubRepository GetRepository(string username, string repositoryName)
        {
            return new GithubRepository(-1, $"{username}/{repositoryName}", "No desc", null, 0);
        }

        public IReadOnlyList<GithubRepository> GetUserRepositories(string username)
        {
            return new List<GithubRepository>();
        }

        public GithubUser GetGithubUser(string githubUsername)
        {
            return new GithubUser(githubUsername, null, "No bio", null);
        }

        public ContributionFullInfo GetUserActivity(string githubUsername)
        {
            return new ContributionFullInfo {PerMonthActivity = new List<ContributionsInfo>()};
        }

        public int GetUserActivity(string githubUsername, DateTime from, DateTime to)
        {
            return default;
        }
    }
}