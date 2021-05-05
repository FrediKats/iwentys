using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Domain.AccountManagement.Dto;

namespace Iwentys.Domain.Quests.Dto
{
    public record QuestInfoDto
    {
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

        public static Expression<Func<Quest, QuestInfoDto>> FromEntity =>
            entity => new QuestInfoDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                Price = entity.Price,
                CreationTime = entity.CreationTime,
                Deadline = entity.Deadline,
                State = entity.State,
                IsOutdated = entity.IsOutdated,
                Author = new IwentysUserInfoDto(entity.Author),
                Executor = entity.Executor == null ? null : new IwentysUserInfoDto(entity.Executor),
                ResponseInfos = entity.Responses.Select(qr => new QuestResponseInfoDto(qr)).ToList()
            };
    }
}