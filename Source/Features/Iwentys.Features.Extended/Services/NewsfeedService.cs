using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Extended;
using Iwentys.Domain.Extended.Models;
using Iwentys.Domain.Study;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Extended.Services
{
    public class NewsfeedService
    {
        private readonly IGenericRepository<SubjectNewsfeed> _subjectNewsfeedRepository;
        private readonly IGenericRepository<Newsfeed> _newsfeedRepository;
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NewsfeedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _subjectRepository = _unitOfWork.GetRepository<Subject>();
            _subjectNewsfeedRepository = _unitOfWork.GetRepository<SubjectNewsfeed>();
            _newsfeedRepository = _unitOfWork.GetRepository<Newsfeed>();
            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
        }

        public async Task<NewsfeedViewModel> CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
        {
            IwentysUser author = await _iwentysUserRepository.GetById(authorizedUser.Id);
            Subject subject = await _subjectRepository.GetById(subjectId);

            SubjectNewsfeed newsfeedEntity;
            if (author.CheckIsAdmin(out SystemAdminUser admin))
            {
                newsfeedEntity = SubjectNewsfeed.CreateAsSystemAdmin(createViewModel, admin, subject);
            }
            else
            {
                Student student = await _studentRepository.GetById(author.Id);
                newsfeedEntity = SubjectNewsfeed.CreateAsGroupAdmin(createViewModel, student.EnsureIsGroupAdmin(), subject);
            }

            _subjectNewsfeedRepository.Insert(newsfeedEntity);
            await _unitOfWork.CommitAsync();

            return await _newsfeedRepository
                .Get()
                .Where(n => n.Id == newsfeedEntity.NewsfeedId)
                .Select(NewsfeedViewModel.FromEntity)
                .SingleAsync();
        }
    }
}