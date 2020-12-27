using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.GithubIntegration.Entities;
using Iwentys.Features.Guilds.Entities;
using Iwentys.Features.Guilds.Tributes.Enums;
using Iwentys.Features.Guilds.Tributes.Models;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Guilds.Tributes.Entities
{
    public class TributeEntity
    {
        public TributeEntity()
        {
        }

        public TributeEntity(GuildEntity guild, GithubProjectEntity projectEntity) : this()
        {
            GuildId = guild.Id;
            ProjectId = projectEntity.Id;
            State = TributeState.Active;
            CreationTimeUtc = DateTime.UtcNow;
        }

        [Key]
        public long ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual GithubProjectEntity Project { get; set; }

        public int GuildId { get; set; }
        public virtual GuildEntity Guild { get; set; }

        public int? MentorId { get; set; }
        public virtual StudentEntity Mentor { get; set; }
        
        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public string Comment { get; set; }
        public DateTime CreationTimeUtc { get; set; }

        public static TributeEntity Create(GuildEntity guild, StudentEntity student, GithubProjectEntity projectEntity, List<TributeEntity> allTributes)
        {
            if (student.GithubUsername != projectEntity.Owner)
                throw InnerLogicException.Tribute.TributeCanBeSendFromStudentAccount(student.Id, projectEntity.Owner);

            if (allTributes.Any(t => t.ProjectId == projectEntity.Id))
                throw InnerLogicException.Tribute.ProjectAlreadyUsed(projectEntity.Id);

            //TODO: fix NRE t.Project.StudentId
            //if (allTributes.Any(t => t.State == TributeState.Active && t.Project.StudentId == student.Id))
            //    throw InnerLogicException.Tribute.UserAlreadyHaveTribute(student.Id);
            
            var tribute = new TributeEntity(guild, projectEntity);
            return tribute;
        }

        public void SetCanceled()
        {
            if (State != TributeState.Active)
                throw InnerLogicException.Tribute.IsNotActive(ProjectId);

            State = TributeState.Canceled;
        }

        public void SetCompleted(int mentorId, TributeCompleteRequest tributeCompleteRequest)
        {
            if (State != TributeState.Active)
                throw InnerLogicException.Tribute.IsNotActive(this.ProjectId);

            MentorId = mentorId;
            DifficultLevel = tributeCompleteRequest.DifficultLevel;
            Mark = tributeCompleteRequest.Mark;
            Comment = tributeCompleteRequest.Comment;
            State = TributeState.Completed;
        }

        public static Expression<Func<TributeEntity, bool>> IsActive => tribute => tribute.State == TributeState.Active;
        public static Expression<Func<TributeEntity, bool>> BelongTo(StudentEntity student) => tribute => tribute.Project.StudentId == student.Id;
    }
}