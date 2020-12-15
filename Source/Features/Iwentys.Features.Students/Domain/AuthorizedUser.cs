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
    }
}