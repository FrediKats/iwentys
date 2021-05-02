using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Study
{
    public class GetStudyGroupByStudent
    {
        public class Query : IRequest<Response>
        {
            public Query(int studentId)
            {
                StudentId = studentId;
            }

            public int StudentId { get; set; }

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
                GroupProfileResponseDto result = await _studentRepository
                    .Get()
                    .Where(sgm => sgm.Id == request.StudentId)
                    .Select(sgm => sgm.Group)
                    .Select(GroupProfileResponseDto.FromEntity)
                    .SingleOrDefaultAsync();

                return new Response(result);
            }
        }
    }
}