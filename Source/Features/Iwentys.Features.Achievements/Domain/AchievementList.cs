using System.Collections.Generic;
using Iwentys.Common.Tools;
using Iwentys.Features.Achievements.Entities;

namespace Iwentys.Features.Achievements.Domain
{
    public static class AchievementList
    {
        public static class Tournaments
        {
            public static readonly Achievement TournamentWinner;

            static Tournaments()
            {
                TournamentWinner = Register(new Achievement
                {
                    Title = "Tournament winner",
                    Description = "Lorem",
                    Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
                });
            }
        }

        public static readonly Achievement AddGithubAchievement;
        public static readonly Achievement BetaTester;
        public static readonly Achievement QuestCreator;
        public static readonly Achievement QuestComplete;
        public static readonly Achievement TestTaskDone;

        private static readonly IdentifierGenerator IdentifierGenerator = new IdentifierGenerator();

        static AchievementList()
        {
            Achievements = new List<Achievement>();
            AddGithubAchievement = Register(new Achievement
            {
                Title = "Add github",
                Description = "Lorem",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            BetaTester = Register(new Achievement
            {
                Title = "TP tester",
                Description = "Lorem",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            QuestCreator = Register(new Achievement
            {
                Title = "Quest creator",
                Description = "For creating quest",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            QuestComplete = Register(new Achievement
            {
                Title = "Quest done",
                Description = "Quest done",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });

            TestTaskDone = Register(new Achievement
            {
                Title = "Test task done",
                Description = "Test task done",
                Url = "https://img.icons8.com/windows/96/000000/open-pokeball.png"
            });
        }

        public static List<Achievement> Achievements { get; }

        private static Achievement Register(Achievement achievement)
        {
            achievement.Id = IdentifierGenerator.Next();
            Achievements.Add(achievement);
            return achievement;
        }
    }
}