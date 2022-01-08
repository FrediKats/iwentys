using System.Collections.Generic;
using System.Linq;
using Iwentys.Domain.Raids;
using Iwentys.Domain.Study;
using Iwentys.Infrastructure.DataAccess.Seeding.FakerEntities.Raids;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.DataAccess.Seeding.EntityGenerators
{
    public class RaidGenerator : IEntityGenerator
    {
        public RaidGenerator(List<Student> students)
        {
            Raids = new List<Raid>();
            RaidVisitors = new List<RaidVisitor>();
            PartySearchRequests = new List<RaidPartySearchRequest>();

            var admin = students.First(s => s.IsAdmin);
            var raidFaker = new RaidFaker();

            var raid = Raid.CreateCommon(admin, raidFaker.CreateRaidCreateArguments());
            raid.Id = 1;
            Raids.Add(raid);

            foreach (Student student in students.Take(10))
            {
                RaidVisitor visitor = raid.RegisterVisitor(student);
                RaidVisitors.Add(visitor);
                PartySearchRequests.Add(raid.CreatePartySearchRequest(visitor, new RaidPartySearchRequestArguments {Description = "Need to add some text"}));
            }
        }

        public List<Raid> Raids { get; set; }
        public List<RaidVisitor> RaidVisitors { get; set; }
        public List<RaidPartySearchRequest> PartySearchRequests { get; set; }

        public void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Raid>().HasData(Raids);
            modelBuilder.Entity<RaidVisitor>().HasData(RaidVisitors);
            modelBuilder.Entity<RaidPartySearchRequest>().HasData(PartySearchRequests);
        }
    }
}