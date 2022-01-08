namespace Iwentys.WebService.Application;

public sealed class AuthorizedUser
{
    private AuthorizedUser(int id)
    {
        Id = id;
    }

    public int Id { get; set; }

    //TODO: think about. Only for debug propose
    public static AuthorizedUser DebugAuth(int id)
    {
        return new AuthorizedUser(id);
    }
}