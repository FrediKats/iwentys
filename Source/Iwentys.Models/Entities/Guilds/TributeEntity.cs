﻿using System;
using System.ComponentModel.DataAnnotations;
using Iwentys.Common.Exceptions;
using Iwentys.Models.Entities.Github;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities.Guilds
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
            CreationTime = DateTime.UtcNow;
        }

        [Key] public long ProjectId { get; set; }

        public GithubProjectEntity ProjectEntity { get; set; }

        public GuildEntity Guild { get; set; }
        public int GuildId { get; set; }

        public TributeState State { get; set; }
        public int? DifficultLevel { get; set; }
        public int? Mark { get; set; }
        public DateTime CreationTime { get; set; }

        public StudentEntity Mentor { get; set; }
        public int? MentorId { get; set; }

        public void SetCanceled()
        {
            State = TributeState.Canceled;
        }

        public void SetCompleted(int mentorId, int difficultLevel, int mark)
        {
            if (State != TributeState.Active)
                throw InnerLogicException.TributeEx.IsNotActive(this.ProjectId);

            MentorId = mentorId;
            DifficultLevel = difficultLevel;
            Mark = mark;
            State = TributeState.Completed;
        }
    }
}