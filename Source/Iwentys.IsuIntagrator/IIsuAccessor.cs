using Iwentys.IsuIntegrator.Models;

namespace Iwentys.IsuIntegrator
{
    public interface IIsuAccessor
    {
        IsuUser GetIsuUser(int isuId, string password);
    }

    //TODO: temp solution
    public class IsuAccessor : IIsuAccessor
    {


        public IsuUser GetIsuUser(int isuId, string password)
        {
            return new IsuUser
            {
                Id = isuId,
                FirstName = "Test",
                MiddleName = "Isu",
                SecondName = "Name"
            };
        }
    }
}