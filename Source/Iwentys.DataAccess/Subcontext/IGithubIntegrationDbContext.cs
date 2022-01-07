﻿using Iwentys.Domain.GithubIntegration;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Subcontext
{
    public interface IGithubIntegrationDbContext
    {
        public DbSet<GithubProject> StudentProjects { get; set; }
        public DbSet<GithubUser> GithubUsersData { get; set; }
    }
}