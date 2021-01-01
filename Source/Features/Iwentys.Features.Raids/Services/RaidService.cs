using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.Raids.Entities;
using Iwentys.Features.Raids.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Raids.Services
{
    public class RaidService
    {
        private IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<Raid> _raidRepository;

        public RaidService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _raidRepository = _unitOfWork.GetRepository<Raid>();
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
    }
}