using Iwentys.Models.Entities;

namespace Iwentys.Core.DomainModel
{
    public class AuthorizedUser
    {
        public int Id { get; set; }
        public Student Profile { get; set; }

        //TODO: remove
        public static AuthorizedUser DebugAuth() => DebugAuth(289140);
        public static AuthorizedUser DebugAuth(int id)
        {
            return new AuthorizedUser {Id = id};
        }
        public static AuthorizedUser DebugAuth(Student profile) => new AuthorizedUser {Id = profile.Id, Profile = profile};

        private AuthorizedUser()
        {
            
        }
    }
}