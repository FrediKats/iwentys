using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.ClientBot.ApiSdk;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Study;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.StudentLeaderboard
{
    public class GetStudentsRatingCommand : IBotCommand
    {
        private readonly IwentysApiProvider _iwentysApi;

        public GetStudentsRatingCommand(IwentysApiProvider iwentysApi)
        {
            _iwentysApi = iwentysApi;
        }

        public bool CanExecute(CommandArgumentContainer args)
        {
            return true;
        }

        public async Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            List<StudyLeaderboardRow> studyLeaderboardRows = await _iwentysApi.LeaderboardApi.GetStudentsRating(null, streamId: int.Parse(args.Arguments[0]), null, null);

            return ResultFormatter.Format(studyLeaderboardRows.Take(20));
        }

        public string CommandName { get; } = nameof(GetStudentsRatingCommand);
        public string Description { get; } = nameof(GetStudentsRatingCommand);
        public string[] Args { get; } = {"Stream"};
    }
}