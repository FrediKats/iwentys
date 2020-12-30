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
    public class Quest
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public int Price { get; init; }
        public DateTime CreationTime { get; init; }
        public DateTime? Deadline { get; init; }
        public QuestState State { get; private set; }

        public int AuthorId { get; init; }
        public virtual Student Author { get; init; }

        public int? ExecutorId { get; private set; }
        public virtual Student Executor { get; private set; }

        public virtual ICollection<QuestResponse> Responses { get; init; }

        public bool IsOutdated => Deadline < DateTime.UtcNow;

        public static Quest New(Student student, CreateQuestRequest createQuest)
        {
            if (student.BarsPoints < createQuest.Price)
                throw InnerLogicException.NotEnoughBarsPoints();

            student.BarsPoints -= createQuest.Price;

            return new Quest
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

        public void Revoke(Student author)
        {
            if (AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermissionFor(author.Id);

            if (State != QuestState.Active)
                throw new InnerLogicException("Quest is not active");

            State = QuestState.Revoked;
            author.BarsPoints += Price;
        }

        public QuestResponse CreateResponse(AuthorizedUser responseAuthor)
        {
            if (AuthorId == responseAuthor.Id)
                throw InnerLogicException.QuestExceptions.AuthorCanRespondToQuest(Id, responseAuthor.Id);
            
            if (State != QuestState.Active || IsOutdated)
                throw new InnerLogicException("Quest is not active");

            return QuestResponse.New(Id, responseAuthor);
        }

        public void MakeCompleted(AuthorizedUser author, Student executor)
        {
            if (AuthorId != author.Id)
                throw InnerLogicException.NotEnoughPermissionFor(author.Id);

            if (State != QuestState.Active || IsOutdated)
                throw InnerLogicException.QuestExceptions.IsNotActive();

            State = QuestState.Completed;
            ExecutorId = executor.Id;
        }
        
        public static Expression<Func<Quest, bool>> IsActive =>
            q => q.State == QuestState.Active
                 && (q.Deadline == null || q.Deadline > DateTime.UtcNow);
        
        public static Expression<Func<Quest, bool>> IsArchived =>
            q => q.State == QuestState.Completed
                 && q.Deadline > DateTime.UtcNow;

        public static Expression<Func<Quest, bool>> IsCompletedBy(AuthorizedUser user) =>
            q => q.State == QuestState.Completed 
                 && q.Responses.Any(r => r.StudentId == user.Id);
    }
}