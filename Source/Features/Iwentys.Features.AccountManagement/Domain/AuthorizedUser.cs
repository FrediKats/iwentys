namespace Iwentys.Features.AccountManagement.Domain
{
    public sealed class AuthorizedUser
    {
        public int Id { get; set; }

        public static AuthorizedUser DebugAuth(int id) => new AuthorizedUser(id);

        private AuthorizedUser(int id)
        {
            Id = id;
        }
    }
}