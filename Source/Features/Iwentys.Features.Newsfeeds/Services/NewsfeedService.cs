using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<SubjectNewsfeed> _subjectNewsfeedRepository;
        private readonly IGenericRepository<GuildNewsfeed> _guildNewsfeedRepository;

        public NewsfeedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _subjectRepository = _unitOfWork.GetRepository<Subject>();
            _guildRepository = _unitOfWork.GetRepository<Guild>();
            _subjectNewsfeedRepository = _unitOfWork.GetRepository<SubjectNewsfeed>();
            _guildNewsfeedRepository = _unitOfWork.GetRepository<GuildNewsfeed>();
        }

        public async Task CreateSubjectNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int subjectId)
        {
            var author = await _studentRepository.GetByIdAsync(authorizedUser.Id);
            var subject = await _subjectRepository.GetByIdAsync(subjectId);

            var newsfeedEntity = SubjectNewsfeed.Create(createViewModel, author, subject);
            
            await _subjectNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateGuildNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int guildId)
        {
            Student author = await _studentRepository.GetByIdAsync(authorizedUser.Id);
            var subject = await _guildRepository.GetByIdAsync(guildId);

            var mentor = await author.EnsureIsGuildMentor(_guildRepository, guildId);
            var newsfeedEntity = GuildNewsfeed.Create(createViewModel, mentor, subject);

            await _guildNewsfeedRepository.InsertAsync(newsfeedEntity);
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