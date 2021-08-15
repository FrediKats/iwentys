﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Raids;
using Iwentys.Domain.Raids.Models;
using Iwentys.Infrastructure.Application;
using Iwentys.Infrastructure.Application.Controllers.Raids.Dtos;
using Iwentys.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Modules.Gamification.Raids
{
    public class RaidService
    {
        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<RaidPartySearchRequest> _raidPartySearchRequestRepository;
        private readonly IGenericRepository<Raid> _raidRepository;
        private readonly IGenericRepository<RaidVisitor> _raidVisitorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RaidService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _raidRepository = _unitOfWork.GetRepository<Raid>();
            _raidVisitorRepository = _unitOfWork.GetRepository<RaidVisitor>();
            _raidPartySearchRequestRepository = _unitOfWork.GetRepository<RaidPartySearchRequest>();
        }

        public async Task<RaidProfileDto> Create(AuthorizedUser user, RaidCreateArguments arguments)
        {
            IwentysUser iwentysUser = await _iwentysUserRepository.GetById(user.Id);

            var raid = Raid.CreateCommon(iwentysUser, arguments);

            _raidRepository.Insert(raid);
            await _unitOfWork.CommitAsync();
            return await _raidRepository
                .Get()
                .Where(r => r.Id == raid.Id)
                .Select(RaidProfileDto.FromEntity)
                .SingleAsync();
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
            IwentysUser student = await _iwentysUserRepository.GetById(user.Id);
            Raid raid = await _raidRepository.GetById(raidId);

            RaidVisitor visitor = raid.RegisterVisitor(student);

            _raidVisitorRepository.Insert(visitor);
            await _unitOfWork.CommitAsync();
        }

        public async Task UnRegisterOnRaid(AuthorizedUser user, int raidId)
        {
            Raid raid = await _raidRepository.GetById(raidId);
            RaidVisitor raidVisitor = await _raidVisitorRepository
                .Get()
                .FirstAsync(rv => rv.Raid == raid && rv.VisitorId == user.Id);

            _raidVisitorRepository.Delete(raidVisitor);
            await _unitOfWork.CommitAsync();
        }

        public async Task ApproveRegistration(AuthorizedUser user, int raidId, int visitorId)
        {
            IwentysUser iwentysUser = await _iwentysUserRepository.GetById(user.Id);
            SystemAdminUser admin = iwentysUser.EnsureIsAdmin();
            RaidVisitor visitor = await _raidVisitorRepository
                .Get()
                .Where(rv => rv.RaidId == raidId && rv.VisitorId == visitorId)
                .SingleAsync();

            visitor.Approve();

            _raidVisitorRepository.Update(visitor);
            await _unitOfWork.CommitAsync();
        }

        public async Task CreatePartySearchRequest(AuthorizedUser user, int raidId, RaidPartySearchRequestArguments arguments)
        {
            IwentysUser student = await _iwentysUserRepository.GetById(user.Id);
            Raid raid = await _raidRepository.GetById(raidId);
            RaidVisitor visitor = await _raidVisitorRepository
                .Get()
                .Where(rv => rv.RaidId == raidId && rv.VisitorId == user.Id)
                .SingleAsync();

            var raidPartySearchRequest = RaidPartySearchRequest.Create(raid, visitor, arguments);

            _raidPartySearchRequestRepository.Insert(raidPartySearchRequest);
            await _unitOfWork.CommitAsync();
        }
    }
}