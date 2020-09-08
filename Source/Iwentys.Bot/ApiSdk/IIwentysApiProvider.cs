using Refit;

namespace Iwentys.ClientBot.ApiSdk
{
    public class IwentysApiProvider
    {
        public IIwentysStudentApi StudentApi { get; set; }

        public IwentysApiProvider(string hostUrl)
        {
            StudentApi = RestService.For<IIwentysStudentApi>(hostUrl);
        }
    }
}