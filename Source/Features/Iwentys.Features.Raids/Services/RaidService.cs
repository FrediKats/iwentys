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
        private readonly IUnitOfWork _unitOfWork;

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

            RaidVisitor visitor = raid.RegisterVisitor(student);

            await _raidVisitorRepository.InsertAsync(visitor);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnRegisterOnRaid(AuthorizedUser user, int raidId)
        {
            Raid raid = await _raidRepository.GetByIdAsync(raidId);
            RaidVisitor raidVisitor = await _raidVisitorRepository
                .Get()
                .FirstAsync(rv => rv.Raid == raid && rv.VisitorId == user.Id);

            _raidVisitorRepository.Delete(raidVisitor);
            await _unitOfWork.CommitAsync();
        }

        public async Task ApproveRegistration(AuthorizedUser user, int raidId, int visitorId)
        {
            Student student = await _studentRepository.GetByIdAsync(user.Id);
            SystemAdminUser admin = student.EnsureIsAdmin();
            RaidVisitor visitor = await _raidVisitorRepository
                .Get()
                .Where(rv => rv.RaidId == raidId && rv.VisitorId == visitorId)
                .SingleAsync();

            visitor.Approve();

            _raidVisitorRepository.Update(visitor);
            await _unitOfWork.CommitAsync();
        }
    }
}