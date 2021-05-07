using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.Services;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildTestTasks
{
    public static class AcceptGuildTestTask
    {
        public class Query : IRequest<Response>
        {
            public Query(int guildId, AuthorizedUser user)
            {
                GuildId = guildId;
                User = user;
            }

            public AuthorizedUser User { get; set; }
            public int GuildId { get; set; }
        }

        public class Response
        {
            public Response(GuildTestTaskInfoResponse testTaskInfo)
            {
                TestTaskInfo = testTaskInfo;
            }

            public GuildTestTaskInfoResponse TestTaskInfo { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<GuildTestTaskSolution> _guildTestTaskSolutionRepository;

            private readonly IUnitOfWork _unitOfWork;
            private readonly IGenericRepository<IwentysUser> _userRepository;


            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _userRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _guildTestTaskSolutionRepository = _unitOfWork.GetRepository<GuildTestTaskSolution>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser author = await _userRepository.GetById(request.User.Id);
                Guild authorGuild = _guildMemberRepository.ReadForStudent(request.User.Id);
                if (authorGuild is null || authorGuild.Id != request.GuildId)
                    throw InnerLogicException.GuildExceptions.IsNotGuildMember(request.User.Id, request.GuildId);

                var testTaskSolution = GuildTestTaskSolution.Create(authorGuild, author);

                _guildTestTaskSolutionRepository.Insert(testTaskSolution);
                await _unitOfWork.CommitAsync();
                return new Response(GuildTestTaskInfoResponse.Wrap(testTaskSolution));
            }
        }
    }
}