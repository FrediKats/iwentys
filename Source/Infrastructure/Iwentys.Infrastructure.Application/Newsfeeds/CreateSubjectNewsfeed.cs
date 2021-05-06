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

namespace Iwentys.Infrastructure.Application.Newsfeeds
{
    public class CreateSubjectNewsfeed
    {
        public class Query : IRequest<Response>
        {
            public NewsfeedCreateViewModel CreateViewModel { get; }
            public AuthorizedUser AuthorizedUser { get; }
            public int SubjectId { get; }

            public Query(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
            {
                CreateViewModel = createViewModel;
                AuthorizedUser = authorizedUser;
                SubjectId = subjectId;
            }
        }

        public class Response
        {
            public Response(NewsfeedViewModel newsfeeds)
            {
                Newsfeeds = newsfeeds;
            }

            public NewsfeedViewModel Newsfeeds { get; }
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
                IwentysUser author = await _iwentysUserRepository.GetById(request.AuthorizedUser.Id);
                Subject subject = await _subjectRepository.GetById(request.SubjectId);

                SubjectNewsfeed newsfeedEntity;
                if (author.CheckIsAdmin(out SystemAdminUser admin))
                {
                    newsfeedEntity = SubjectNewsfeed.CreateAsSystemAdmin(request.CreateViewModel, admin, subject);
                }
                else
                {
                    Student student = await _studentRepository.GetById(author.Id);
                    newsfeedEntity = SubjectNewsfeed.CreateAsGroupAdmin(request.CreateViewModel, student.EnsureIsGroupAdmin(), subject);
                }

                _subjectNewsfeedRepository.Insert(newsfeedEntity);
                await _unitOfWork.CommitAsync();

                NewsfeedViewModel result = await _newsfeedRepository
                    .Get()
                    .Where(n => n.Id == newsfeedEntity.NewsfeedId)
                    .Select(NewsfeedViewModel.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}