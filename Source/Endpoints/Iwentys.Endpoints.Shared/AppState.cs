namespace Iwentys.Endpoints.OldShared
{
    public class AppState
    {
        public int GetUserId() => UserId;

        public int UserId { get; set; }
        public string AccessToken { get; set; }
    }
}