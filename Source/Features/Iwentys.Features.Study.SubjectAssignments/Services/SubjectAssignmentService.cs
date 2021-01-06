﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Features.AccountManagement.Domain;
using Iwentys.Features.AccountManagement.Entities;
using Iwentys.Features.Study.Entities;
using Iwentys.Features.Study.SubjectAssignments.Entities;
using Iwentys.Features.Study.SubjectAssignments.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.SubjectAssignments.Services
{
    public class SubjectAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<SubjectAssignment> _subjectAssignmentRepository;
        private readonly IGenericRepository<SubjectAssignmentSubmit> _subjectAssignmentSubmitRepository;
        private readonly IGenericRepository<GroupSubjectAssignment> _groupSubjectAssignmentRepository;
        private readonly IGenericRepository<GroupSubject> _groupSubjectRepository;

        public SubjectAssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _subjectAssignmentRepository = _unitOfWork.GetRepository<SubjectAssignment>();
            _subjectAssignmentSubmitRepository = _unitOfWork.GetRepository<SubjectAssignmentSubmit>();
            _groupSubjectAssignmentRepository = _unitOfWork.GetRepository<GroupSubjectAssignment>();
            _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubject>();
        }

        //TODO: if user is not teacher - filter submits
        public async Task<List<SubjectAssignmentDto>> GetSubjectAssignmentForGroup(AuthorizedUser user, int groupId)
        {
            return await _groupSubjectAssignmentRepository
                .Get()
                .Where(gsa => gsa.GroupId == groupId)
                .Select(gsa => gsa.SubjectAssignment)
                .Distinct()
                .Select(SubjectAssignmentDto.FromEntity)
                .ToListAsync();
        }
        
        public async Task<List<SubjectAssignmentDto>> GetSubjectAssignmentForSubject(AuthorizedUser user, int subjectId)
        {
            return await _subjectAssignmentRepository
                .Get()
                .Where(sa => sa.SubjectId == subjectId)
                .Select(SubjectAssignmentDto.FromEntity)
                .ToListAsync();
        }

        //TODO: here must be subject id and we need to resolve group subject
        //TODO: OR list of group subject ids
        public async Task<SubjectAssignmentDto> CreateSubjectAssignment(AuthorizedUser user, int subjectId, SubjectAssignmentCreateArguments arguments)
        {
            IwentysUser iwentysUser = await _iwentysUserRepository.GetById(user.Id);
            List<GroupSubject> groupSubjects = await _groupSubjectRepository
                .Get()
                .Where(gs => gs.SubjectId == subjectId)
                .ToListAsync();

            //TODO: looks like hack
            SubjectAssignment subjectAssignment = null;
                //TODO: check if count == 0
            foreach (GroupSubject groupSubject in groupSubjects.Take(1))
            {
                subjectAssignment = SubjectAssignment.Create(iwentysUser, groupSubject, arguments);
                await _subjectAssignmentRepository.InsertAsync(subjectAssignment);
            }

            await _unitOfWork.CommitAsync();
            return await _subjectAssignmentRepository
                .Get()
                .Where(sa => sa.Id == subjectAssignment.Id)
                .Select(SubjectAssignmentDto.FromEntity)
                .SingleAsync();
        }

        //TODO: filter for student
        //TODO: add pagination
        public async Task<List<SubjectAssignmentSubmitDto>> SearchSubjectAssignmentSubmits(AuthorizedUser user, SubjectAssignmentSubmitSearchArguments searchArguments)
        {
            return await _subjectAssignmentSubmitRepository
                .Get()
                .Where(sas => sas.SubjectAssignment.SubjectId == searchArguments.SubjectId)
                .WhereIf(searchArguments.StudentId,sas => sas.StudentId == searchArguments.StudentId)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .ToListAsync();
        }

        public async Task<SubjectAssignmentSubmitDto> SendSubmit(AuthorizedUser user, int subjectAssignmentId, SubjectAssignmentSubmitCreateArguments arguments)
        {
            SubjectAssignment subjectAssignment = await _subjectAssignmentRepository.GetById(subjectAssignmentId);

            SubjectAssignmentSubmit subjectAssignmentSubmit = subjectAssignment.CreateSubmit(user, arguments);

            await _subjectAssignmentSubmitRepository.InsertAsync(subjectAssignmentSubmit);
            await _unitOfWork.CommitAsync();

            return await _subjectAssignmentSubmitRepository
                .Get()
                .Where(sas => sas.Id == subjectAssignmentSubmit.Id)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .SingleAsync();
        }

        public async Task<SubjectAssignmentSubmitDto> GetSubjectAssignmentSubmit(AuthorizedUser user, int subjectAssignmentSubmitId)
        {
            return await _subjectAssignmentSubmitRepository
                .Get()
                .Where(sas => sas.Id == subjectAssignmentSubmitId)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .SingleAsync();
        }

        //TODO: check permission
        public async Task SendFeedback(AuthorizedUser user, int subjectAssignmentSubmitId, SubjectAssignmentSubmitFeedbackArguments assignment)
        {
            SubjectAssignmentSubmit subjectAssignmentSubmit = await _subjectAssignmentSubmitRepository.GetById(subjectAssignmentSubmitId);
            IwentysUser iwentysUser = await _iwentysUserRepository.GetById(user.Id);

            subjectAssignmentSubmit.ApplyFeedback(iwentysUser, assignment);

            _subjectAssignmentSubmitRepository.Update(subjectAssignmentSubmit);
            await _unitOfWork.CommitAsync();
        }
    }
}