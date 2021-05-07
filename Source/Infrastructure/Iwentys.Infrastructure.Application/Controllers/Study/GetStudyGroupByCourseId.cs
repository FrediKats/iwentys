using System.Collections.Generic;
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
    public class GetStudyGroupByCourseId
    {
        public class Query : IRequest<Response>
        {
            public int? CourseId { get; set; }

            public Query(int? courseId)
            {
                CourseId = courseId;
            }
        }

        public class Response
        {
            public Response(List<GroupProfileResponseDto> groups)
            {
                Groups = groups;
            }

            public List<GroupProfileResponseDto> Groups { get; set; }
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
                List<GroupProfileResponseDto> result = await _studyGroupRepository
                    .Get()
                    .WhereIf(request.CourseId, gs => gs.StudyCourseId == request.CourseId)
                    .Select(GroupProfileResponseDto.FromEntity)
                    .ToListAsync();

                return new Response(result);
            }
        }
    }
}