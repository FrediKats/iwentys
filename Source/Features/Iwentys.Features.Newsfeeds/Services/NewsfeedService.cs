using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Guilds.Domain;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Newsfeeds.Entities;
using Iwentys.Features.Newsfeeds.Models;
using Iwentys.Features.Study.Domain;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Newsfeeds.Services
{
    public class NewsfeedService
    {
        private readonly IGenericRepository<GuildNewsfeed> _guildNewsfeedRepository;
        private readonly IGenericRepository<SubjectNewsfeed> _subjectNewsfeedRepository;
        private readonly IGenericRepository<Newsfeed> _newsfeedRepository;
        private readonly IGenericRepository<Guild> _guildRepository;
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<Student> _studentRepository;
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
                newsfeedEntity = SubjectNewsfeed.Create(createViewModel, admin, subject);
            }
            else
            {
                Student student = await _studentRepository.GetById(author.Id);
                newsfeedEntity = SubjectNewsfeed.Create(createViewModel, student.EnsureIsGroupAdmin(), subject);
            }

            await _subjectNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();

            return await Get(newsfeedEntity.NewsfeedId);
        }

        public async Task<NewsfeedViewModel> CreateGuildNewsfeed(NewsfeedCreateViewModel createViewModel, AuthorizedUser authorizedUser, int guildId)
        {
            Student author = await _studentRepository.GetById(authorizedUser.Id);
            Guild subject = await _guildRepository.GetById(guildId);

            GuildMentor mentor = await author.EnsureIsGuildMentor(_guildRepository, guildId);
            var newsfeedEntity = GuildNewsfeed.Create(createViewModel, mentor, subject);

            await _guildNewsfeedRepository.InsertAsync(newsfeedEntity);
            await _unitOfWork.CommitAsync();

            return await Get(newsfeedEntity.NewsfeedId);
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

        public async Task<NewsfeedViewModel> Get(int newsfeedId)
        {
            return await _newsfeedRepository
                .Get()
                .Where(n => n.Id == newsfeedId)
                .Select(NewsfeedViewModel.FromEntity)
                .SingleAsync();
        }
    }
}