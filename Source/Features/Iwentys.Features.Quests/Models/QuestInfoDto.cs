using Iwentys.Features.Quests.Entities;
using Iwentys.Features.Quests.Enums;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Common.Tools;
using Iwentys.Features.AccountManagement.Models;

namespace Iwentys.Features.Quests.Models
{
    public record QuestInfoDto
    {
        public QuestInfoDto(Quest quest)
            :  this(
                quest.Id,
                quest.Title,
                quest.Description,
                quest.Price,
                quest.CreationTime,
                quest.Deadline,
                quest.State,
                quest.IsOutdated,
                new IwentysUserInfoDto(quest.Author),
                quest.Executor == null ? null : new IwentysUserInfoDto(quest.Executor),
                //TODO: AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
                quest.Responses?.SelectToList(qr => new QuestResponseInfoDto(qr)))
        {
        }

        public QuestInfoDto(int id, string title, string description, int price, DateTime creationTime, DateTime? deadline, QuestState state, bool isOutdated, IwentysUserInfoDto author, IwentysUserInfoDto executor,
            List<QuestResponseInfoDto> responseInfos)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            CreationTime = creationTime;
            Deadline = deadline;
            State = state;
            IsOutdated = isOutdated;
            Author = author;
            Executor = executor;
            ResponseInfos = responseInfos;
        }

        public QuestInfoDto()
        {
        }
        
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int Price { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime? Deadline { get; init; }
        public QuestState State { get; init; }
        public bool IsOutdated { get; init; }
        public IwentysUserInfoDto Author { get; init; }
        public IwentysUserInfoDto Executor { get; init; }
        public List<QuestResponseInfoDto> ResponseInfos { get; set; }

        public static Expression<Func<Quest, QuestInfoDto>> FromEntity => entity => new QuestInfoDto(entity);
    }
}