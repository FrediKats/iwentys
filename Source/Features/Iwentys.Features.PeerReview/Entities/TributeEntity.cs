using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Models.Guilds;
using Iwentys.Features.Students.Entities;
using Iwentys.Features.Tributes.Enums;

namespace Iwentys.Features.Tributes.Entities
{
    public class TributeEntity
    {
        public TributeEntity()
        {
        }

        public TributeEntity(GuildEntity guild, GithubProjectEntity projectEntity) : this()
        {
            GuildId = guild.Id;
            ProjectEntity = projectEntity;
            ProjectId = projectEntity.Id;
            State = TributeState.Active;
            CreationTimeUtc = DateTime.UtcNow;
        }

        [Key] public long ProjectId { get; set; }
        public virtual GithubProjectEntity ProjectEntity { get; set; }

        public int GuildId { get; set; }
        public virtual GuildEntity Guild { get; set; }

        public int? MentorId { get; set; }
        public virtual StudentEntity Mentor { get; set; }
        
        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public static TributeEntity Create(GuildEntity guild, StudentEntity student, GithubProjectEntity projectEntity, List<TributeEntity> allTributes)
        {
            if (student.GithubUsername != projectEntity.Owner)
                throw InnerLogicException.Tribute.TributeCanBeSendFromStudentAccount(student.Id, projectEntity.Owner);

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.Tribute.ProjectAlreadyUsed(projectEntity.Id);

            if (allTributes.Any(t => t.State == TributeState.Active && t.ProjectEntity.StudentId == student.Id))
                throw InnerLogicException.Tribute.UserAlreadyHaveTribute(student.Id);
            
            var tribute = new TributeEntity(guild, projectEntity);
            return tribute;
        }

        public void SetCanceled()
        {
            if (State != TributeState.Active)
                throw InnerLogicException.Tribute.IsNotActive(ProjectId);

            State = TributeState.Canceled;
        }

        public void SetCompleted(int mentorId, int difficultLevel, int mark)
        {
            if (State != TributeState.Active)
                throw InnerLogicException.Tribute.IsNotActive(this.ProjectId);

            MentorId = mentorId;
            DifficultLevel = difficultLevel;
            Mark = mark;
            State = TributeState.Completed;
        }
    }
}