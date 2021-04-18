using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Gamification;
using MediatR;

namespace Iwentys.Features.Gamification.Karmas
{
    public static class SendKarma
    {
        public class Query : IRequest<Response>
        {
            public int StudentId { get; }
            public AuthorizedUser AuthorizedUser { get; }

            public Query(int studentId, AuthorizedUser authorizedUser)
            {
                StudentId = studentId;
                AuthorizedUser = authorizedUser;
            }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<KarmaUpVote> _karmaRepository;

            private readonly IGenericRepository<IwentysUser> _studentRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _karmaRepository = unitOfWork.GetRepository<KarmaUpVote>();
                _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            }


            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser target = await _studentRepository.GetById(request.StudentId);

                var karmaUpVote = KarmaUpVote.Create(request.AuthorizedUser, target);

                _karmaRepository.Insert(karmaUpVote);
                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}