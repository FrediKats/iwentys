using Bogus;

namespace Iwentys.EntityManager.DataSeeding;

public class FakerSingleton
{
    public static readonly Faker Instance = new Faker();
}