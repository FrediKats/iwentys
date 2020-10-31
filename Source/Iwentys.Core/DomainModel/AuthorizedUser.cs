using System.Threading.Tasks;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;

namespace Iwentys.Core.DomainModel
{
    public sealed class AuthorizedUser
    {
        public int Id { get; set; }

        public static AuthorizedUser DebugAuth(int id) => new AuthorizedUser {Id = id};
        private AuthorizedUser() {}

        public Task<StudentEntity> GetProfile(StudentRepository repository)
        {
            return repository.ReadByIdAsync(Id);
        }
    }
}