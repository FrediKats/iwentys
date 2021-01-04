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
        private readonly IGenericRepository<GuildNewsfeed> _guildNewsfeedRepository;
        private readonly IGenericRepository<Guild> _guildRepository;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<SubjectNewsfeed> _subjectNewsfeedRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

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
            Student author = await _studentRepository.GetById(authorizedUser.Id);
            Subject subject = await _subjectRepository.GetById(subjectId);
            StudyGroup studyGroup = author.GroupMember?.Group;

            var newsfeedEntity = SubjectNewsfeed.Create(createViewModel, author, subject, studyGroup);

            await _subjectNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreateGuildNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int guildId)
        {
            Student author = await _studentRepository.GetById(authorizedUser.Id);
            Guild subject = await _guildRepository.GetById(guildId);

            GuildMentor mentor = await author.EnsureIsGuildMentor(_guildRepository, guildId);
            var newsfeedEntity = GuildNewsfeed.Create(createViewModel, mentor, subject);

            await _guildNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();
        }

        public async Task<List<NewsfeedViewModel>> GetSubjectNewsfeeds(int subjectId)
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