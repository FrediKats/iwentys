using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;

namespace Iwentys.Core.DomainModel
{
    public class AuthorizedUser
    {
        public int Id { get; set; }

        public StudentEntity GetProfile(IStudentRepository repository)
        {
            return repository.ReadById(Id);
        }

        //TODO: remove
        public static AuthorizedUser DebugAuth() => DebugAuth(289140);
        public static AuthorizedUser DebugAuth(int id) => new AuthorizedUser {Id = id};
        private AuthorizedUser() {}
    }
}