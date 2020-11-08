using System;
using IdentityServer4.EntityFramework.Options;
using Iwentys.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Iwentys.Tests.Tools
{
    public class TestDatabaseProvider
    {
        public static IwentysDbContext GetDatabaseContext()
        {
            DbContextOptions<IwentysDbContext> options = new DbContextOptionsBuilder<IwentysDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new IwentysDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}