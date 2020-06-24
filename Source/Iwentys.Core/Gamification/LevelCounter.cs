using System;
using System.Collections.Generic;
using System.Linq;

namespace Iwentys.Core.Gamification
{
    public class LevelCounter
    {
        private const int MaxLevel = 35;
        private const int ExperienceCounterExponentBase = 2;
        private const double ExperienceCounterExponentDefaultStep = 5.7;
        private const double ExperienceCounterExponentStepPerLevelIncrease = 1 / 20.0;

        private static readonly List<int> ExperienceForLevel = Enumerable.Range(0, MaxLevel + 1).Select(ConvertLevelToExperienceBound).ToList();

        public int CurrentExperience { get; }
        public int Level => ExperienceForLevel.FindLastIndex(exp => exp <= CurrentExperience);
        public int ExperienceToNextLevel => ExperienceForLevel[Level + 1];

        public LevelCounter(int currentExperience)
        {
            CurrentExperience = currentExperience;
        }

        public static int ConvertLevelToExperienceBound(int level)
        {
            return (int) (level * Math.Pow(ExperienceCounterExponentBase, ExperienceCounterExponentDefaultStep + level * ExperienceCounterExponentStepPerLevelIncrease));
        }
    }
}