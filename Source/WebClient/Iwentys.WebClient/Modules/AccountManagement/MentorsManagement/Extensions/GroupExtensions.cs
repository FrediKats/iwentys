using Iwentys.Sdk;

namespace Iwentys.WebClient.Modules.AccountManagement.MentorsManagement.Extensions
{
    public static class GroupExtensions
    {
        public static bool HasMentor(this GroupMentorsDto groupMentors, MentorDto mentor) 
            => groupMentors.Mentors.Contains(mentor);
    }
}