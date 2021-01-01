using System.Collections.Generic;
using System.Linq;
using Iwentys.Database.Seeding.FakerEntities.Raids;
using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Database.Seeding.EntityGenerators
{
    public class RaidGenerator : IEntityGenerator
    {
        public RaidGenerator(List<Student> students)
        {
            Raids = new List<Raid>();
            RaidVisitors = new List<RaidVisitor>();

            SystemAdminUser admin = students.First(s => s.Role == StudentRole.Admin).EnsureIsAdmin();
            var raidFaker = new RaidFaker();

            var raid = Raid.CreateCommon(admin, raidFaker.CreateRaidCreateArguments());
            raid.Id = 1;
            Raids.Add(raid);

            foreach (Student student in students.Take(10))
                RaidVisitors.Add(raid.RegisterVisitor(admin.Student, student));
        }

        public List<Raid> Raids { get; set; }
        public List<RaidVisitor> RaidVisitors { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Raid>().HasData(Raids);
            modelBuilder.Entity<RaidVisitor>().HasData(RaidVisitors);
        }
    }
}