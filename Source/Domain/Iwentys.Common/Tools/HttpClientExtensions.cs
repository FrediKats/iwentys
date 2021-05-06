using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Iwentys.Common.Tools
{
    public static class HttpClientExtensions
    {
        public static async Task<TValue> FindFromJsonAsync<TValue>(this HttpClient client, string requestUri)
        {
            HttpResponseMessage taskResponse = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            using (taskResponse)
            {
                if (taskResponse.StatusCode == HttpStatusCode.NotFound)
                    return default;

                taskResponse.EnsureSuccessStatusCode();
                return await taskResponse.Content!.ReadFromJsonAsync<TValue>();
            }
        }
    }
}