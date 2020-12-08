using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Students.Domain;
using Iwentys.Features.Students.Entities;

namespace Iwentys.Features.Quests.Entities
{
    public class QuestEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? Deadline { get; set; }
        public QuestState State { get; set; }

        public int AuthorId { get; set; }
        public StudentEntity Author { get; set; }

        public List<QuestResponseEntity> Responses { get; set; }

        public bool IsOutdated => Deadline < DateTime.UtcNow;

        public static QuestEntity New(string title, string description, int price, DateTime? deadline, StudentEntity author)
        {
            return new QuestEntity
            {
                Title = title,
                Description = description,
                Price = price,
                CreationTime = DateTime.UtcNow,
                Deadline = deadline,
                State = QuestState.Active,
                AuthorId = author.Id
            };
        }

        public void Revoke(AuthorizedUser author)
        {
            if (AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermission(author.Id);

            if (State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            State = QuestState.Revoked;
        }
        
        public static Expression<Func<QuestEntity, bool>> IsActive => q => q.State == QuestState.Active && (q.Deadline == null || q.Deadline > DateTime.UtcNow);
        public static Expression<Func<QuestEntity, bool>> IsArchived => q => q.State == QuestState.Completed || q.Deadline > DateTime.UtcNow;
    }
}