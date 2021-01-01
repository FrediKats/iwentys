using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Raids.Models;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Raids.Services
{
    public class RaidService
    {
        private IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Student> _studentRepository;
        private readonly IGenericRepository<Raid> _raidRepository;
        private readonly IGenericRepository<RaidVisitor> _raidVisitorRepository;

        public RaidService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _studentRepository = _unitOfWork.GetRepository<Student>();
            _raidRepository = _unitOfWork.GetRepository<Raid>();
            _raidVisitorRepository = _unitOfWork.GetRepository<RaidVisitor>();
        }

        public async Task<List<RaidProfileDto>> Get()
        {
            return await _raidRepository
                .Get()
                .Select(RaidProfileDto.FromEntity)
                .ToListAsync();
        }

        public async Task<RaidProfileDto> Get(int raidId)
        {
            return await _raidRepository
                .Get()
                .Where(r => r.Id == raidId)
                .Select(RaidProfileDto.FromEntity)
                .SingleAsync();
        }

        public async Task RegisterOnRaid(AuthorizedUser user, int raidId)
        {
            Student student = await _studentRepository.GetByIdAsync(user.Id);
            Raid raid = await _raidRepository.GetByIdAsync(raidId);

            RaidVisitor visitor = raid.RegisterVisitor(student, student);

            await _raidVisitorRepository.InsertAsync(visitor);
            await _unitOfWork.CommitAsync();
        }
    }
}