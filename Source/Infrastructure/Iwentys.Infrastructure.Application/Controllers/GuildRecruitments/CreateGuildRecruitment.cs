using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildRecruitments
{
    public static class CreateGuildRecruitment
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; set; }
            public int GuildId { get; set; }
            public GuildRecruitmentCreateArguments Arguments { get; set; }

            public Query(AuthorizedUser authorizedUser, int guildId, GuildRecruitmentCreateArguments arguments)
            {
                AuthorizedUser = authorizedUser;
                GuildId = guildId;
                Arguments = arguments;
            }
        }

        public class Response
        {
            public Response(GuildRecruitmentInfoDto guildRecruitment)
            {
                GuildRecruitment = guildRecruitment;
            }

            public GuildRecruitmentInfoDto GuildRecruitment { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildRecruitment> _guildRecruitmentRepository;
            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;

            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildRecruitmentRepository = _unitOfWork.GetRepository<GuildRecruitment>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                Guild guild = _guildRepository.GetById(request.GuildId).Result;
                IwentysUser user = await _iwentysUserRepository.GetById(request.AuthorizedUser.Id);

                var guildRecruitment = GuildRecruitment.Create(user, guild, request.Arguments);

                _guildRecruitmentRepository.Insert(guildRecruitment);
                await _unitOfWork.CommitAsync();
                return new Response(GuildRecruitmentInfoDto.FromEntity.Compile().Invoke(guildRecruitment));
            }
        }
    }
}