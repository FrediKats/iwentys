using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Students.Enums;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<SubjectNewsfeed> _subjectNewsfeedRepository;
        private readonly IGenericRepository<GuildNewsfeed> _guildNewsfeedRepository;

        public NewsfeedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _subjectRepository = _unitOfWork.GetRepository<Subject>();
            _subjectNewsfeedRepository = _unitOfWork.GetRepository<SubjectNewsfeed>();
            _guildNewsfeedRepository = _unitOfWork.GetRepository<GuildNewsfeed>();
        }

        public async Task CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
        {
            var author = await _studentRepository.FindByIdAsync(authorizedUser.Id);
            var subject = await _subjectRepository.FindByIdAsync(subjectId);

            if (author.Role != StudentRole.GroupAdmin && author.Role != StudentRole.Admin)
                throw InnerLogicException.NotEnoughPermissionFor(author.Id);

            var newsfeedEntity = SubjectNewsfeed.Create(createViewModel, author, subject);
            
            await _subjectNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<NewsfeedViewModel>> GetSubjectNewsfeedsAsync(int subjectId)
        {
            return await _subjectNewsfeedRepository
                .Get()
                .Where(sn => sn.SubjectId == subjectId)
                .Select(NewsfeedViewModel.FromSubjectEntity)
                .ToListAsync();
        }

        public async Task<List<NewsfeedViewModel>> GetGuildNewsfeeds(int guildId)
        {
            return await _guildNewsfeedRepository.Get()
                .Where(gn => gn.GuildId == guildId)
                .Select(NewsfeedViewModel.FromGuildEntity)
                .ToListAsync();
        }
    }
}