using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Tools;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<StudentEntity> _studentRepository;
        private readonly IGenericRepository<SubjectEntity> _subjectRepository;
        private readonly IGenericRepository<SubjectNewsfeedEntity> _subjectNewsfeedRepository;
        private readonly IGenericRepository<GuildNewsfeedEntity> _guildNewsfeedRepository;

        public NewsfeedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<StudentEntity>();
            _subjectRepository = _unitOfWork.GetRepository<SubjectEntity>();
            _subjectNewsfeedRepository = _unitOfWork.GetRepository<SubjectNewsfeedEntity>();
            _guildNewsfeedRepository = _unitOfWork.GetRepository<GuildNewsfeedEntity>();
        }

        public async Task CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
        {
            var author = await _studentRepository.GetByIdAsync(authorizedUser.Id);
            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            var newsfeedEntity = SubjectNewsfeedEntity.Create(createViewModel, author, subject);
            
            await _subjectNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<NewsfeedViewModel>> GetSubjectNewsfeedsAsync(int subjectId)
        {
            List<SubjectNewsfeedEntity> subjectNewsfeedEntities = await _subjectNewsfeedRepository.GetAsync()
                .Where(sn => sn.SubjectId == subjectId)
                .ToListAsync();
            
            return subjectNewsfeedEntities.SelectToList(n => NewsfeedViewModel.Wrap(n.Newsfeed));
        }

        public async Task<List<NewsfeedViewModel>> GetGuildNewsfeeds(int guildId)
        {
            List<GuildNewsfeedEntity> guildNewsfeedEntities = await _guildNewsfeedRepository.GetAsync()
                .Where(gn => gn.GuildId == guildId)
                .ToListAsync();

            return guildNewsfeedEntities.SelectToList(n => NewsfeedViewModel.Wrap(n.Newsfeed));
        }
    }
}