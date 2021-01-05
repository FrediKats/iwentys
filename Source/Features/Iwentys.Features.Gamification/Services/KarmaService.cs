using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Study.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Services
{
    public class KarmaService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _studentRepository;
        private readonly IGenericRepository<KarmaUpVote> _karmaRepository;

        public KarmaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<IwentysUser>();
            _karmaRepository = _unitOfWork.GetRepository<KarmaUpVote>();
        }

        public async Task<KarmaStatistic> GetStatistic(int studentId)
        {
            List<KarmaUpVote> karmaUpVotes = await _karmaRepository
                .Get()
                .Where(karma => karma.TargetId == studentId)
                .ToListAsync();

            return KarmaStatistic.Create(studentId, karmaUpVotes);
        }

        public async Task UpVote(AuthorizedUser author, int targetId)
        {
            IwentysUser target = await _studentRepository.GetById(targetId);

            var karmaUpVote = KarmaUpVote.Create(author, target);

            await _karmaRepository.InsertAsync(karmaUpVote);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveUpVote(AuthorizedUser author, int targetId)
        {
            IwentysUser target = await _studentRepository.GetById(targetId);
            KarmaUpVote upVote = await _karmaRepository.Get().FirstAsync(k => k.AuthorId == author.Id && k.TargetId == target.Id);

            _karmaRepository.Delete(upVote);
            await _unitOfWork.CommitAsync();
        }
    }
}