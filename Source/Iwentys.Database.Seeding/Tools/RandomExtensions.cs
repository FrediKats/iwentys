using System;
using System.Collections.Generic;

namespace Iwentys.Database.Seeding.Tools
{
    public static class RandomExtensions
    {
        public static readonly Random Instance = new Random();

        public static T GetRandom<T>(this List<T> collection)
        {
            return collection[Instance.Next(collection.Count)];
        }
    }
}