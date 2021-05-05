using System;
using Iwentys.Database;
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
                    .Options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}