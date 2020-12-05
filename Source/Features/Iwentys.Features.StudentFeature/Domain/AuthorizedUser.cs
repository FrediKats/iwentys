using System.Threading.Tasks;
using Iwentys.Common.Tools;
using Iwentys.Features.StudentFeature.Entities;
using Iwentys.Features.StudentFeature.Repositories;

namespace Iwentys.Features.StudentFeature.Domain
{
    //TODO: probably need to move to ...?
    public sealed class AuthorizedUser
    {
        public int Id { get; set; }

        public static AuthorizedUser DebugAuth(int id) => new AuthorizedUser {Id = id};
        private AuthorizedUser() {}

        public Task<StudentEntity> GetProfile(IStudentRepository repository)
        {
            return repository.GetAsync(Id);
        }
    }
}