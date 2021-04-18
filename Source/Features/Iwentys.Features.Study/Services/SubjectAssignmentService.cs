using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iwentys.Common.Databases;
using Iwentys.Domain.AccountManagement;
using Iwentys.Domain.Study;
using Iwentys.Domain.Study.Models;
using Microsoft.EntityFrameworkCore;

namespace Iwentys.Features.Study.Services
{
    public class SubjectAssignmentService
    {
        private readonly IGenericRepository<GroupSubjectAssignment> _groupSubjectAssignmentRepository;
        private readonly IGenericRepository<GroupSubject> _groupSubjectRepository;

        private readonly IGenericRepository<IwentysUser> _iwentysUserRepository;
        private readonly IGenericRepository<Assignment> _assignmentRepository;
        private readonly IGenericRepository<StudentAssignment> _studentAssignmentRepository;
        private readonly IGenericRepository<SubjectAssignment> _subjectAssignmentRepository;
        private readonly IGenericRepository<SubjectAssignmentSubmit> _subjectAssignmentSubmitRepository;
        private readonly IGenericRepository<Subject> _subjectRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SubjectAssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            _iwentysUserRepository = _unitOfWork.GetRepository<IwentysUser>();
            _subjectAssignmentRepository = _unitOfWork.GetRepository<SubjectAssignment>();
            _subjectAssignmentSubmitRepository = _unitOfWork.GetRepository<SubjectAssignmentSubmit>();
            _groupSubjectAssignmentRepository = _unitOfWork.GetRepository<GroupSubjectAssignment>();
            _groupSubjectRepository = _unitOfWork.GetRepository<GroupSubject>();
            _subjectRepository = _unitOfWork.GetRepository<Subject>();
            _assignmentRepository = _unitOfWork.GetRepository<Assignment>();
            _studentAssignmentRepository = _unitOfWork.GetRepository<StudentAssignment>();
        }

        public async Task<SubjectAssignmentSubmitDto> SendSubmit(AuthorizedUser user, SubjectAssignmentSubmitCreateArguments arguments)
        {
            SubjectAssignment subjectAssignment = await _subjectAssignmentRepository.GetById(arguments.SubjectAssignmentId);

            SubjectAssignmentSubmit subjectAssignmentSubmit = subjectAssignment.CreateSubmit(user, arguments);

            _subjectAssignmentSubmitRepository.Insert(subjectAssignmentSubmit);
            await _unitOfWork.CommitAsync();

            return await _subjectAssignmentSubmitRepository
                .Get()
                .Where(sas => sas.Id == subjectAssignmentSubmit.Id)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .SingleAsync();
        }

        public async Task SendFeedback(AuthorizedUser user, SubjectAssignmentSubmitFeedbackArguments arguments)
        {
            SubjectAssignmentSubmit subjectAssignmentSubmit = await _subjectAssignmentSubmitRepository.GetById(arguments.SubjectAssignmentSubmitId);
            IwentysUser iwentysUser = await _iwentysUserRepository.GetById(user.Id);

            subjectAssignmentSubmit.ApplyFeedback(iwentysUser, arguments);

            _subjectAssignmentSubmitRepository.Update(subjectAssignmentSubmit);
            await _unitOfWork.CommitAsync();
        }

        private async Task<List<SubjectAssignmentSubmitDto>> SearchSubjectAssignmentSubmits(SubjectAssignmentSubmitSearchArguments searchArguments)
        {
            return await _subjectAssignmentSubmitRepository
                .Get()
                .Where(sas => sas.SubjectAssignment.SubjectId == searchArguments.SubjectId)
                .WhereIf(searchArguments.StudentId, sas => sas.StudentId == searchArguments.StudentId)
                .Select(sas => new SubjectAssignmentSubmitDto(sas))
                .ToListAsync();
        }
    }
}