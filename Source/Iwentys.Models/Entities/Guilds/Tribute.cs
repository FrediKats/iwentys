using Iwentys.Models.Exceptions;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Models.Entities.Guilds
{
    public class Tribute
    {
        public Guild Guild { get; set; }
        public int GuildId { get; set; }

        public StudentProject Project { get; set; }
        public int ProjectId { get; set; }

        public TributeState State { get; set; }
        public int DifficultLevel { get; set; }
        public int Mark { get; set; }
        public Student Totem { get; set; }
        public int TotemId { get; set; }

        public static Tribute New(int guildId, int projectId)
        {
            return new Tribute
            {
                GuildId = guildId,
                ProjectId = projectId,
                State = TributeState.Created
            };
        }

        public void SetCanceled()
        {
            State = TributeState.Canceled;
        }

        public void SetCompleted(int totemId, int difficultLevel, int mark)
        {
            if (State == TributeState.Created)
                throw new InnerLogicException($"Can't completed tribute. It's in state [{State}]");

            TotemId = totemId;
            DifficultLevel = difficultLevel;
            Mark = mark;
            State = TributeState.Completed;
        }
    }
}