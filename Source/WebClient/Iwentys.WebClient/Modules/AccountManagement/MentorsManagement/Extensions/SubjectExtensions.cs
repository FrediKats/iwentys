using System.Linq;
using Iwentys.Sdk;

namespace Iwentys.WebClient.Modules.AccountManagement.MentorsManagement.Extensions
{
    public static class SubjectExtensions
    {
        public static bool HasMentor(this SubjectMentorsDto subjectMentors, MentorDto mentor)
            => subjectMentors.Groups.Any(g => g.HasMentor(mentor));
    }
}