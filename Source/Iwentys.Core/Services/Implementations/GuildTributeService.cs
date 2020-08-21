using System.Linq;
using Iwentys.Core.DomainModel;
using Iwentys.Core.Services.Abstractions;
using Iwentys.Database.Context;
using Iwentys.Database.Repositories;
using Iwentys.Models.Entities;
using Iwentys.Models.Entities.Guilds;
using Iwentys.Models.Exceptions;
using Iwentys.Models.Tools;
using Iwentys.Models.Transferable.GuildTribute;
using Iwentys.Models.Types.Guilds;

namespace Iwentys.Core.Services.Implementations
{
    public class GuildTributeService : IGuildTributeService
    {
        private readonly DatabaseAccessor _databaseAccessor;

        public GuildTributeService(DatabaseAccessor databaseAccessor)
        {
            _databaseAccessor = databaseAccessor;
        }

        public TributeInfoDto[] GetPendingTributes(AuthorizedUser user)
        {
            Guild guild = _databaseAccessor.GuildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _databaseAccessor.TributeRepository
                .ReadForGuild(guild.Id)
                .Where(t => t.State == TributeState.Active)
                .Select(TributeInfoDto.Wrap)
                .ToArray();
        }

        public TributeInfoDto[] GetStudentTributeResult(AuthorizedUser user)
        {
            Guild guild = _databaseAccessor.GuildRepository.ReadForStudent(user.Id) ?? throw InnerLogicException.Guild.IsNotGuildMember(user.Id, null);

            return _databaseAccessor.TributeRepository
                .ReadStudentInGuildTributes(guild.Id, user.Id)
                .Select(TributeInfoDto.Wrap)
                .ToArray();
        }

        public TributeInfoDto CreateTribute(AuthorizedUser user, int projectId)
        {
            Student student = _databaseAccessor.Student.Get(user.Id);
            Guild guild = _databaseAccessor.GuildRepository.ReadForStudent(student.Id);
            StudentProject project = _databaseAccessor.StudentProjectRepository.Get(projectId);
            Tribute[] allTributes = _databaseAccessor.TributeRepository.Read().ToArray();

            if (allTributes.Any(t => t.ProjectId == projectId))
                throw InnerLogicException.TributeEx.ProjectAlreadyUsed(projectId);

            if (allTributes.Any(t => t.State == TributeState.Active && t.Project.StudentId == student.Id))
                throw InnerLogicException.TributeEx.UserAlreadyHaveTribute(user.Id);

            var tribute = Tribute.New(guild.Id, project.Id);
            tribute.Project = project;
            return _databaseAccessor.TributeRepository.Create(tribute).To(TributeInfoDto.Wrap);
        }

        public TributeInfoDto CancelTribute(AuthorizedUser user, int tributeId)
        {
            Student student = user.GetProfile(_databaseAccessor.Student);
            Tribute tribute = _databaseAccessor.TributeRepository.Get(tributeId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            if (tribute.Project.StudentId == user.Id)
            {
                tribute.SetCanceled();
            }
            else
            {
                student.EnsureIsMentor(_databaseAccessor.GuildRepository, tribute.GuildId);
                tribute.SetCanceled();
            }

            return _databaseAccessor.TributeRepository.Update(tribute).To(TributeInfoDto.Wrap);
        }

        public TributeInfoDto CompleteTribute(AuthorizedUser user, TributeCompleteDto tributeCompleteDto)
        {
            Student student = user.GetProfile(_databaseAccessor.Student);
            Tribute tribute = _databaseAccessor.TributeRepository.Get(tributeCompleteDto.TributeId);
            GuildMentorUser mentor = student.EnsureIsMentor(_databaseAccessor.GuildRepository, tribute.GuildId);

            if (tribute.State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(tribute);

            tribute.SetCompleted(mentor.Student.Id, tributeCompleteDto.DifficultLevel, tributeCompleteDto.Mark);
            return _databaseAccessor.TributeRepository.Update(tribute).To(TributeInfoDto.Wrap);
        }
    }
}