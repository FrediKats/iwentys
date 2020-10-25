using System.Collections.Generic;
using Iwentys.Core.DomainModel;

namespace Iwentys.Endpoints.ClientBot.Tools
{
    public class UserIdentifier
    {
        private readonly Dictionary<int, int> _usersByTelegramId;

        public UserIdentifier()
        {
            _usersByTelegramId = new Dictionary<int, int>();
        }

        public void SetUser(int messengerUserId, int studentId)
        {
            _usersByTelegramId[messengerUserId] = studentId;
        }

        public AuthorizedUser GetUser(int messengerUserId)
        {
            return AuthorizedUser.DebugAuth(_usersByTelegramId.TryGetValue(messengerUserId, out int studentId)
                ? studentId
                : 228617);
        }
    }
}