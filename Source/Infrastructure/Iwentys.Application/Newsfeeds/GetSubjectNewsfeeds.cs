using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Guilds;
using Iwentys.Domain.Newsfeeds;
using Iwentys.Domain.Newsfeeds.Dto;
using Iwentys.Domain.Study;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Newsfeeds
{
    public class GetSubjectNewsfeeds
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser AuthorizedUser { get; }
            public int SubjectId { get; }

            public Query(AuthorizedUser authorizedUser, int subjectId)
            {
                AuthorizedUser = authorizedUser;
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public List<NewsfeedViewModel> Newsfeeds { get; set; }

            public Response(List<NewsfeedViewModel> newsfeeds)
            {
                Newsfeeds = newsfeeds;
            }

        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<GuildNewsfeed> _guildNewsfeedRepository;
            private readonly IGenericRepository<SubjectNewsfeed> _subjectNewsfeedRepository;
            private readonly IGenericRepository<Newsfeed> _newsfeedRepository;
            private readonly IGenericRepository<Guild> _guildRepository;
            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
            private readonly IGenericRepository<Student> _studentRepository;
            private readonly IGenericRepository<Subject> _subjectRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _studentRepository = _unitOfWork.GetRepository<Student>();
                _subjectRepository = _unitOfWork.GetRepository<Subject>();
                _guildRepository = _unitOfWork.GetRepository<Guild>();
                _subjectNewsfeedRepository = _unitOfWork.GetRepository<SubjectNewsfeed>();
                _guildNewsfeedRepository = _unitOfWork.GetRepository<GuildNewsfeed>();
                _newsfeedRepository = _unitOfWork.GetRepository<Newsfeed>();
                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                List<NewsfeedViewModel> result = await _subjectNewsfeedRepository
                    .Get()
                    .Where(sn => sn.SubjectId == request.SubjectId)
                    .Select(NewsfeedViewModel.FromSubjectEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}