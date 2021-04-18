using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Models;
using MediatR;

namespace Iwentys.Features.Guilds.Tournaments
{
    public static class CreateCodeMarathon
    {
        public class Query : IRequest<Response>
        {
            public Query(AuthorizedUser user, CreateCodeMarathonTournamentArguments arguments)
            {
                User = user;
                Arguments = arguments;
            }

            public AuthorizedUser User { get; set; }
            public CreateCodeMarathonTournamentArguments Arguments { get; set; }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<CodeMarathonTournament> _codeMarathonTournamentRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
                _codeMarathonTournamentRepository = _unitOfWork.GetRepository<CodeMarathonTournament>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser user = await _studentRepository.GetById(request.User.Id);

                var codeMarathonTournamentEntity = CodeMarathonTournament.Create(user, request.Arguments);

                _codeMarathonTournamentRepository.Insert(codeMarathonTournamentEntity);
                await _unitOfWork.CommitAsync();

                return new Response();
            }

        }
    }
}