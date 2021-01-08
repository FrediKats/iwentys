using Iwentys.Features.Study.SubjectAssignments.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Flurl.Http;

namespace Iwentys.Endpoint.Sdk.ControllerClients.Study
{
    public class SubjectAssignmentControllerClient
    {
        public SubjectAssignmentControllerClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }

        public Task<List<SubjectAssignmentDto>> GetAssignmentForGroup(int groupId)
        {
            return Client.GetFromJsonAsync<List<SubjectAssignmentDto>>($"api/subject-assignment/for-group/{groupId}");
        }

        public Task<List<SubjectAssignmentDto>> GetSubjectAssignmentForSubject(int subjectId)
        {
            return Client.GetFromJsonAsync<List<SubjectAssignmentDto>>($"api/subject-assignment/for-subject/{subjectId}");
        }

        public Task<SubjectAssignmentSubmitDto> GetSubjectAssignmentSubmit(int subjectId, int subjectAssignmentSubmitId)
        {
            return new FlurlClient(Client)
                .Request($"api/subject-assignment/{subjectId}/submits/{subjectAssignmentSubmitId}")
                .GetJsonAsync<SubjectAssignmentSubmitDto>();
        }

        public Task<List<SubjectAssignmentSubmitDto>> GetStudentSubjectAssignmentSubmits(int subjectId)
        {
            return new FlurlClient(Client)
                .Request($"api/subject-assignment/{subjectId}/submits")
                .GetJsonAsync<List<SubjectAssignmentSubmitDto>>();
        }

        #region Teacher only method

        public Task CreateSubjectAssignment(int subjectId, SubjectAssignmentCreateArguments arguments)
        {
            return new FlurlClient(Client)
                .Request($"api/subject-assignment/management/{subjectId}")
                .PostJsonAsync(arguments);
        }

        public Task<List<SubjectAssignmentSubmitDto>> SearchSubjectAssignmentSubmits(int subjectId, int? studentId = null)
        {
            return new FlurlClient(Client)
                .Request($"api/subject-assignment/management/{subjectId}/submits")
                .SetQueryParam("studentId", studentId)
                .GetJsonAsync<List<SubjectAssignmentSubmitDto>>();
        }

        public Task SendFeedback(int subjectId, int subjectAssignmentSubmitId, SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            return new FlurlClient(Client)
                .Request($"api/subject-assignment/management/{subjectId}/submits/{subjectAssignmentSubmitId}")
                .PutJsonAsync(arguments);
        }

        #endregion
    }
}