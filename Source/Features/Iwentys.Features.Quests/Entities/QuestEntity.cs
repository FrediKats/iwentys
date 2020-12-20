using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common.Exceptions;
using Iwentys.Features.Quests.Enums;
using Iwentys.Features.Quests.Models;
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
        public virtual StudentEntity Author { get; set; }

        public int? ExecutorId { get; set; }
        public virtual StudentEntity Executor { get; set; }

        public virtual ICollection<QuestResponseEntity> Responses { get; set; }

        public bool IsOutdated => Deadline < DateTime.UtcNow;

        public static QuestEntity New(StudentEntity student, CreateQuestRequest createQuest)
        {
            if (student.BarsPoints < createQuest.Price)
                throw InnerLogicException.NotEnoughBarsPoints();

            student.BarsPoints -= createQuest.Price;

            return new QuestEntity
            {
                Title = createQuest.Title,
                Description = createQuest.Description,
                Price = createQuest.Price,
                CreationTime = DateTime.UtcNow,
                Deadline = createQuest.Deadline,
                State = QuestState.Active,
                AuthorId = student.Id
            };
        }

        public void Revoke(StudentEntity student)
        {
            if (AuthorId != student.Id)
                throw InnerLogicException.NotEnoughPermissionFor(student.Id);

            if (State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            State = QuestState.Revoked;
        }

        public QuestResponseEntity CreateResponse(AuthorizedUser responseAuthor)
        {
            if (State != QuestState.Active || IsOutdated)
                throw new InnerLogicException("Quest is not active");

            return QuestResponseEntity.New(Id, responseAuthor);
        }

        public void MakeCompleted(AuthorizedUser author, StudentEntity executor)
        {
            if (AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermissionFor(author.Id);

            if (State != QuestState.Active || IsOutdated)
                throw InnerLogicException.Quest.IsNotActive();

            State = QuestState.Completed;
            ExecutorId = executor.Id;
        }
        
        public static Expression<Func<QuestEntity, bool>> IsActive =>
            q => q.State == QuestState.Active
                 && (q.Deadline == null || q.Deadline > DateTime.UtcNow);
        
        public static Expression<Func<QuestEntity, bool>> IsArchived =>
            q => q.State == QuestState.Completed
                 || q.Deadline > DateTime.UtcNow;

        public static Expression<Func<QuestEntity, bool>> IsCompletedBy(AuthorizedUser user) =>
            q => q.State == QuestState.Completed 
                 && q.Responses.Any(r => r.StudentId == user.Id);
    }
}