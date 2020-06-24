using Microsoft.EntityFrameworkCore;

using System;
using Iwentys.Database.Context;

namespace Iwentys.Tests.Tools
{
    public class TestDatabaseProvider
    {
        public static IwentysDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<IwentysDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new IwentysDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}