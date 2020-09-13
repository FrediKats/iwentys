using System.Threading.Tasks;
using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public interface IIwentysDebugCommandApi
    {
        [Get("/api/DebugCommand/loginOrCreate/{userId}")]
        public Task<string> LoginOrCreate(int userId);
    }
}