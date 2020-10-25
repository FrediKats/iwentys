using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using Iwentys.Core.Services;
using Iwentys.Models;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.Study;
using Microsoft.Extensions.DependencyInjection;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Core;

namespace Iwentys.ClientBot.Commands.StudentLeaderboard
{
    public class GetStudentsRatingCommand : IBotCommand
    {
        private readonly StudyLeaderboardService _leaderboardService;

        public GetStudentsRatingCommand(ServiceProvider serviceProvider)
        {
            _leaderboardService = serviceProvider.GetService<StudyLeaderboardService>();
        }

        public Result CanExecute(CommandArgumentContainer args)
        {
            if (args.Arguments.Count != Args.Length)
                return Result.Fail("Wrong argument count");

            if (!int.TryParse(args.Arguments[0], out _))
                return Result.Fail("Argument must be int value (courseId)");

            return Result.Ok();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            var searchDto = new StudySearchParameters
            {
                CourseId = int.Parse(args.Arguments[0])
            };

            List<StudyLeaderboardRow> studyLeaderboardRows = _leaderboardService.GetStudentsRatings(searchDto);

            return ResultFormatter.FormatToTask(studyLeaderboardRows.Take(20));
        }

        public string CommandName { get; } = nameof(GetStudentsRatingCommand);
        public string Description { get; } = nameof(GetStudentsRatingCommand);
        public string[] Args { get; } = {"CourseId"};
    }
}