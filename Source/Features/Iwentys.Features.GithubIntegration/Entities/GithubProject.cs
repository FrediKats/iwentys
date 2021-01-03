﻿using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.GithubIntegration.Models;

namespace Iwentys.Features.GithubIntegration.Entities
{
    public class GithubProject
    {
        public GithubProject()
        {
        }

        public GithubProject(IwentysUser owner, GithubRepositoryInfoDto githubRepositoryInfoDto) : this()
        {
            Id = githubRepositoryInfoDto.Id;
            Owner = owner.GithubUsername;
            Description = githubRepositoryInfoDto.Description;
            FullUrl = githubRepositoryInfoDto.Url;
            Name = githubRepositoryInfoDto.Name;
            OwnerUserId = owner.Id;
        }

        public long Id { get; set; }
        public string FullUrl { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StarCount { get; set; }
        public long GithubRepositoryId { get; set; }

        public int OwnerUserId { get; set; }
        public virtual IwentysUser OwnerUser { get; set; }
    }
}