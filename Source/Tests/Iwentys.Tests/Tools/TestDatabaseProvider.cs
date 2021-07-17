using System;
using Iwentys.Infrastructure.DataAccess;
using Iwentys.Infrastructure.DataAccess.Seeding;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Tests.Tools
{
    public static class TestDatabaseProvider
    {
        public static IwentysDbContext GetDatabaseContext()
        {
            var databaseContext = new IwentysDbContext(
                new DbContextOptionsBuilder<IwentysDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .UseLazyLoadingProxies()
                    .Options,
                new DatabaseContextGenerator());
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}