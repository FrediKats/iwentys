using System.Collections.Generic;
using System.Threading.Tasks;
using Iwentys.ClientBot.ApiSdk;
using Iwentys.Core.DomainModel;

namespace Iwentys.ClientBot.Tools
{
    public class UserIdentifier
    {
        private readonly Dictionary<int, int> _usersByTelegramId;
        private readonly Dictionary<int, IwentysApiProvider> _userProviders;

        public UserIdentifier()
        {
            _userProviders = new Dictionary<int, IwentysApiProvider>();
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

        public async Task<IwentysApiProvider> GetProvider(int messengerUserId, IwentysApiProvider apiProvider)
        {
            var user = GetUser(messengerUserId);
            if (!_userProviders.ContainsKey(user.Id))
            {
                var token = await apiProvider.Client.ApiDebugcommandLoginorcreateAsync(user.Id).ConfigureAwait(false);
                _userProviders[user.Id] = new IwentysApiProvider(token.Token);
            }

            return _userProviders[user.Id];
        }
    }
}