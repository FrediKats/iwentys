using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Core.Gamification
{
    public class LevelCounter
    {
        private const int MaxLevel = 35;
        
        private static readonly List<int> ExperienceForLevel = Enumerable.Range(0, MaxLevel + 1).Select(ConvertLevelToExperienceBound).ToList();

        public int CurrentExperience { get; }
        public int Level => ExperienceForLevel.FindLastIndex(exp => exp <= CurrentExperience);
        public int ExperienceToNextLevel => ExperienceForLevel[Level + 1];

        public LevelCounter(int currentExperience)
        {
            CurrentExperience = currentExperience;
        }

        public static int ConvertLevelToExperienceBound(int level) => (int)(level * Math.Pow(2, 5.7 + level / 20.0));
    }
}