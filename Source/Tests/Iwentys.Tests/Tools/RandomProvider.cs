using System;
using Bogus;

namespace Iwentys.Tests.Tools
{
    public static class RandomProvider
    {
        public static readonly Random Random = new Random();
        public static readonly Faker Faker = new Faker();
    }
}