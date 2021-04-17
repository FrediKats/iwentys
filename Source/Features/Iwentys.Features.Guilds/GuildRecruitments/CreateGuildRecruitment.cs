using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using Iwentys.Domain.Services;
using MediatR;

namespace Iwentys.Features.Guilds.GuildRecruitments
{
    public class CreateGuildRecruitment
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

        public class Handler : RequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildRecruitmentMember> _guildRecruitmentMemberRepository;
            private readonly IGenericRepository<GuildRecruitment> _guildRecruitmentRepository;

            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _unitOfWork = unitOfWork;
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _guildRecruitmentRepository = _unitOfWork.GetRepository<GuildRecruitment>();
                _guildRecruitmentMemberRepository = _unitOfWork.GetRepository<GuildRecruitmentMember>();
            }

            protected override Response Handle(Query request)
            {
                Guild guild = _guildRepository.GetById(request.GuildId).Result;
                GuildMember creator = guild.Members.Find(m => m.MemberId == request.AuthorizedUser.Id) ?? throw EntityNotFoundException.Create(typeof(GuildMember), request.AuthorizedUser.Id);

                var guildRecruitment = GuildRecruitment.Create(creator.Member.EnsureIsGuildMentor(guild), guild, request.Arguments);

                _guildRecruitmentRepository.InsertAsync(guildRecruitment).Wait();
                _unitOfWork.CommitAsync().Wait();

                return new Response(GuildRecruitmentInfoDto.FromEntity.Compile().Invoke(guildRecruitment));
            }
        }
    }
}