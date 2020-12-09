using System;
using Iwentys.Database.Context;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;

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
            EntityFrameworkManager.ContextFactory = context => new IwentysDbContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}