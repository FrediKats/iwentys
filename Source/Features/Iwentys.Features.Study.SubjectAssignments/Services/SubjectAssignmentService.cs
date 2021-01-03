using System.Collections.Generic;
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
        public async Task CreateSubjectAssignment(AuthorizedUser user, int subjectId, SubjectAssignmentCreateArguments arguments)
        {
            IwentysUser iwentysUser = await _iwentysUserRepository.GetByIdAsync(user.Id);
            List<GroupSubject> groupSubjects = await _groupSubjectRepository
                .Get()
                .Where(gs => gs.SubjectId == subjectId)
                .ToListAsync();

            //TODO: looks like hack
            foreach (GroupSubject groupSubject in groupSubjects.Take(1))
            {
                var subjectAssignment = SubjectAssignment.Create(iwentysUser, groupSubject, arguments);
                await _subjectAssignmentRepository.InsertAsync(subjectAssignment);
            }

            await _unitOfWork.CommitAsync();
        }

        //TODO: add search arguments
        //TODO: filter for student
        //TODO: add pagination
        public async Task<List<SubjectAssignmentSubmitDto>> GetSubjectAssignmentSubmits(AuthorizedUser user, int subjectId)
        {
            return await _subjectAssignmentSubmitRepository
                .Get()
                .Where(sas => sas.SubjectAssignment.SubjectId == subjectId)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .ToListAsync();
        }

        public async Task SendSubmit(AuthorizedUser user, int subjectAssignmentId, SubjectAssignmentSubmitCreateArguments arguments)
        {
            SubjectAssignment subjectAssignment = await _subjectAssignmentRepository.GetByIdAsync(subjectAssignmentId);

            SubjectAssignmentSubmit subjectAssignmentSubmit = subjectAssignment.CreateSubmit(user, arguments);

            await _subjectAssignmentSubmitRepository.InsertAsync(subjectAssignmentSubmit);
            await _unitOfWork.CommitAsync();
        }

        public async Task SendFeedback(AuthorizedUser user, int subjectAssignmentSubmitId, SubjectAssignmentSubmitFeedbackArguments assignment)
        {
            SubjectAssignmentSubmit subjectAssignmentSubmit = await _subjectAssignmentSubmitRepository.GetByIdAsync(subjectAssignmentSubmitId);
            IwentysUser iwentysUser = await _iwentysUserRepository.GetByIdAsync(user.Id);

            subjectAssignmentSubmit.ApplyFeedback(iwentysUser, assignment);
        }
    }
}