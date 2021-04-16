using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain;
using Iwentys.Domain.Gamification;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Karmas
{
    public static class RevokeKarma
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
                KarmaUpVote upVote = await _karmaRepository.Get().FirstAsync(k => k.AuthorId == request.AuthorizedUser.Id && k.TargetId == target.Id, cancellationToken);

                _karmaRepository.Delete(upVote);
                await _unitOfWork.CommitAsync();

                return new Response();
            }
        }
    }
}