using System.Collections.Generic;
using Iwentys.Common.Tools;

namespace Iwentys.Domain.Gamification
{
    public static class AchievementList
    {
        public static readonly Achievement AddGithubAchievement;
        public static readonly Achievement BetaTester;
        public static readonly Achievement QuestCreator;
        public static readonly Achievement QuestComplete;
        public static readonly Achievement TestTaskDone;

        private static readonly IdentifierGenerator IdentifierGenerator = new IdentifierGenerator();

        static AchievementList()
        {
            Achievements = new List<Achievement>();
            AddGithubAchievement = Register(
                "Add github",
                "Lorem",
                "https://img.icons8.com/windows/96/000000/open-pokeball.png");

            BetaTester = Register(
                "TP tester",
                "Lorem",
                "https://img.icons8.com/fluent-systems-regular/96/000000/bug.png");

            QuestCreator = Register(
                "Quest creator",
                "For creating quest",
                "https://img.icons8.com/windows/96/000000/open-pokeball.png");

            QuestComplete = Register(
                "Quest done",
                "Quest done",
                "https://img.icons8.com/windows/96/000000/open-pokeball.png");

            TestTaskDone = Register(
                "Test task done",
                "Test task done",
                "https://img.icons8.com/windows/96/000000/open-pokeball.png");
        }

        public static List<Achievement> Achievements { get; }

        private static Achievement Register(string title, string description, string url)
        {
            var achievement = new Achievement
            {
                Id = IdentifierGenerator.Next(),
                Title = title,
                Description = description,
                ImageUrl = url
            };

            Achievements.Add(achievement);
            return achievement;
        }

        public static class Tournaments
        {
            public static readonly Achievement TournamentWinner;

            static Tournaments()
            {
                TournamentWinner = Register(
                    "Tournament winner",
                    "Lorem",
                    "https://img.icons8.com/windows/96/000000/open-pokeball.png");
            }
        }
    }
}