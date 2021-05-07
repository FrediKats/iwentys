using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Infrastructure.Application.Controllers.Study
{
    public class GetStudyGroupByName
    {
        public class Query : IRequest<Response>
        {
            public Query(string groupName)
            {
                GroupName = groupName;
            }

            public string GroupName { get; set; }

        }

        public class Response
        {
            public Response(GroupProfileResponseDto @group)
            {
                Group = @group;
            }

            public GroupProfileResponseDto Group { get; set; }
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
                var name = new GroupName(request.GroupName);
                var result = await _studyGroupRepository
                    .Get()
                    .Where(StudyGroup.IsMatch(name))
                    .Select(GroupProfileResponseDto.FromEntity)
                    .SingleAsync();

                return new Response(result);
            }
        }
    }
}