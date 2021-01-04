namespace Iwentys.Features.AccountManagement.Domain
{
    public sealed class AuthorizedUser
    {
        private AuthorizedUser(int id)
        {
            Id = id;
        }

        public int Id { get; set; }

        //FYI: Only for debug propose
        public static AuthorizedUser DebugAuth(int id)
        {
            return new AuthorizedUser(id);
        }
    }
}