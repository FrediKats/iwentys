using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Gamification.Entities;
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

        public Task<List<StudyLeaderboardRowDto>> GetStudyRating(int courseId)
        {
            return Client.GetFromJsonAsync<List<StudyLeaderboardRowDto>>($"api/leaderboard/study?courseId={courseId}");
        }

        public Task CourseRatingForceRefresh(int courseId)
        {
            return Client.GetAsync($"api/leaderboard/force-update?courseId={courseId}");
        }

        public Task<CourseLeaderboardRow> FindStudentLeaderboardPosition(int studentId)
        {
            return Client.FindFromJsonAsync<CourseLeaderboardRow>($"api/leaderboard/student-position?studentId={studentId}");
        }

        public Task<StudentActivityInfoDto> GetStudentActivity(int studentId)
        {
            return Client.GetFromJsonAsync<StudentActivityInfoDto>($"api/leaderboard/activity/{studentId}");
        }
    }
}