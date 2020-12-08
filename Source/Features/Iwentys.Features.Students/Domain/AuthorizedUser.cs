using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Repositories;

namespace Iwentys.Features.Students.Domain
{
    //TODO: probably need to move to ...?
    public sealed class AuthorizedUser
    {
        public int Id { get; set; }

        public static AuthorizedUser DebugAuth(int id) => new AuthorizedUser(id);

        private AuthorizedUser(int id)
        {
            Id = id;
        }

        public Task<StudentEntity> GetProfile(IStudentRepository repository)
        {
            return repository.GetAsync(Id);
        }
    }
}