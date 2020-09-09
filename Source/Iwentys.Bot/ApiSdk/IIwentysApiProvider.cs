using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public class IwentysApiProvider
    {
        public IIwentysStudentApi StudentApi { get; set; }
        public IIwentysDebugCommandApi DebugCommand { get; set; }

        public IwentysApiProvider(string hostUrl)
        {
            StudentApi = RestService.For<IIwentysStudentApi>(hostUrl);
            DebugCommand = RestService.For<IIwentysDebugCommandApi>(hostUrl);
        }
    }
}