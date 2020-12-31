using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Gamification.Entities;
using Iwentys.Features.Gamification.Models;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Gamification.Services
{
    public class KarmaService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<KarmaUpVote> _karmaRepository;

        public KarmaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
        }

        public async Task<KarmaStatistic> GetStatistic(int studentId)
        {
            List<KarmaUpVote> karmaUpVotes = await _karmaRepository
                .Get()
                .Where(karma => karma.TargetId == studentId)
                .ToListAsync();

            return KarmaStatistic.Create(studentId, karmaUpVotes);
        }
    }
}