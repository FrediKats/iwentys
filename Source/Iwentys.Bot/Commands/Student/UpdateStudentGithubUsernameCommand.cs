using System.Threading.Tasks;
using Tef.BotFramework.Abstractions;
using Tef.BotFramework.Common;

namespace Iwentys.ClientBot.Commands.Student
{
    public class UpdateStudentGithubUsernameCommand : IBotCommand
    {
        public bool CanExecute(CommandArgumentContainer args)
        {
            throw new System.NotImplementedException();
        }

        public Task<Result<string>> ExecuteAsync(CommandArgumentContainer args)
        {
            throw new System.NotImplementedException();
        }

        public string CommandName { get; }
        public string Description { get; }
        public string[] Args { get; }
    }
}