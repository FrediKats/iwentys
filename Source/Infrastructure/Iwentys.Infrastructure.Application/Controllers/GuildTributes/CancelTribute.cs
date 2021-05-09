using System.Threading;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.GithubIntegration;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Guilds.Models;
using Iwentys.Infrastructure.Application.Controllers.GithubIntegration;
using Iwentys.Infrastructure.DataAccess;
using MediatR;

namespace Iwentys.Infrastructure.Application.Controllers.GuildTributes
{
    public class CancelTribute
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, long tributeId)
            {
                User = user;
                TributeId = tributeId;
            }

            public AuthorizedUser User { get; set; }
            public long TributeId { get; set; }
        }

        public class Response
        {
            public Response(TributeInfoResponse tribute)
            {
                Tribute = tribute;
            }

            public TributeInfoResponse Tribute { get; set; }
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly GithubIntegrationService _githubIntegrationService;
            private readonly IGenericRepository<GuildMember> _guildMemberRepository;
            private readonly IGenericRepository<Guild> _guildRepositoryNew;
            private readonly IGenericRepository<Tribute> _guildTributeRepository;
            private readonly IGenericRepository<GithubProject> _studentProjectRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork, GithubIntegrationService githubIntegrationService)
            {
                _githubIntegrationService = githubIntegrationService;
                _unitOfWork = unitOfWork;
                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
                _guildRepositoryNew = _unitOfWork.GetRepository<Guild>();
                _guildMemberRepository = _unitOfWork.GetRepository<GuildMember>();
                _studentProjectRepository = _unitOfWork.GetRepository<GithubProject>();
                _guildTributeRepository = _unitOfWork.GetRepository<Tribute>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser student = _studentRepository.GetById(request.User.Id).Result;
                Tribute tribute = _guildTributeRepository.GetById(request.TributeId).Result;

                if (tribute.Project.OwnerUserId == request.User.Id)
                {
                    tribute.SetCanceled();
                }
                else
                {
                    //TODO: move logic to domain
                    Guild guild = await _guildRepositoryNew.GetById(tribute.GuildId);
                    student.EnsureIsGuildMentor(guild);
                    tribute.SetCanceled();
                }

                _guildTributeRepository.Update(tribute);
                _unitOfWork.CommitAsync().Wait();
                return new Response(TributeInfoResponse.Wrap(tribute));
            }
        }
    }
}