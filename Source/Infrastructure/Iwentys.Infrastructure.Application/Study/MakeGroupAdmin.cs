using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using MediatR;

namespace Iwentys.Infrastructure.Application.Study
{
    public class MakeGroupAdmin
    {
        public class Query : IRequest<Response>
        {
            public AuthorizedUser Initiator { get; set; }
            public int NewGroupAdminId { get; set; }

            public Query(AuthorizedUser initiator, int newGroupAdminId)
            {
                Initiator = initiator;
                NewGroupAdminId = newGroupAdminId;
            }
        }

        public class Response
        {
        }

        public class Handler : IRequestHandler<Query, Response>
        {
            private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
            private readonly IGenericRepository<Student> _studentRepository;
            private readonly IGenericRepository<StudyGroup> _studyGroupRepository;
            private readonly IGenericRepository<StudyCourse> _studyCourseRepository;
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;

                _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
                _studentRepository = _unitOfWork.GetRepository<Student>();
                _studyGroupRepository = _unitOfWork.GetRepository<StudyGroup>();
                _studyCourseRepository = _unitOfWork.GetRepository<StudyCourse>();
            }

            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                IwentysUser initiatorProfile = await _iwentysUserRepository.GetById(request.Initiator.Id);
                Student newGroupAdminProfile = await _studentRepository.GetById(request.NewGroupAdminId);

                StudyGroup studyGroup = StudyGroup.MakeGroupAdmin(initiatorProfile, newGroupAdminProfile);

                _studyGroupRepository.Update(studyGroup);

                return new Response();
            }
        }
    }
}