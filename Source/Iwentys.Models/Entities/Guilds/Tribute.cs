using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class Tribute
    {
        public Guild Guild { get; set; }
        public int GuildId { get; set; }

        public Student Totem { get; set; }
        public int TotemId { get; set; }

        public StudentProject Project { get; set; }
        public int ProjectId { get; set; }

        public TributeState State { get; set; }

        public static Tribute New(int guildId, int totemId, int projectId)
        {
            return new Tribute
            {
                GuildId = guildId,
                TotemId = totemId,
                ProjectId = projectId,
                State = TributeState.Created
            };
        }
    }
}