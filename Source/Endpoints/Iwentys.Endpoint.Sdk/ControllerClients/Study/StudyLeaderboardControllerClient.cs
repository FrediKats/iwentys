using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Study.Models;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class StudyLeaderboardControllerClient
    {
        public StudyLeaderboardControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<StudyLeaderboardRow>> GetStudyRating(int courseId)
        {
            return Client.GetFromJsonAsync<List<StudyLeaderboardRow>>($"api/StudyLeaderboard/study-rate?courseId={courseId}");
        }

        public Task<StudentActivityInfoDto> GetStudentActivity(int studentId)
        {
            return Client.GetFromJsonAsync<StudentActivityInfoDto>($"api/StudyLeaderboard/activity/{studentId}");
        }
    }
}