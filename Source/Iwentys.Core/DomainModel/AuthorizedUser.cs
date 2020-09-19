using Iwentys.Database.Repositories.Abstractions;
using Iwentys.Models.Entities;

namespace Iwentys.Core.DomainModel
{
    public sealed class AuthorizedUser
    {
        public int Id { get; set; }

        public static AuthorizedUser DebugAuth(int id) => new AuthorizedUser {Id = id};
        private AuthorizedUser() {}

        public StudentEntity GetProfile(IStudentRepository repository)
        {
            return repository.ReadById(Id);
        }
    }
}