using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iwentys.Common;
using Iwentys.Domain.AccountManagement;

namespace Iwentys.Domain.Quests;

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
    public virtual IwentysUser Author { get; init; }

    public int? ExecutorId { get; private set; }
    public virtual IwentysUser Executor { get; private set; }
    public int ExecutorMark { get; set; }

    public virtual ICollection<QuestResponse> Responses { get; init; }

    public bool IsOutdated => Deadline < DateTime.UtcNow;

    public static Expression<Func<Quest, bool>> IsActive =>
        q => q.State == QuestState.Active
             && (q.Deadline == null || q.Deadline > DateTime.UtcNow);

    public static Expression<Func<Quest, bool>> IsArchived =>
        q => q.State == QuestState.Completed
             && q.Deadline > DateTime.UtcNow;

    public static Quest New(IwentysUser student, CreateQuestRequest createQuest)
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

    public void Revoke(IwentysUser author)
    {
        if (AuthorId != author.Id)
            throw InnerLogicException.NotEnoughPermissionFor(author.Id);

        if (State != QuestState.Active)
            throw InnerLogicException.QuestExceptions.IsNotActive();

        State = QuestState.Revoked;
        author.BarsPoints += Price;
    }

    public QuestResponse CreateResponse(IwentysUser responseAuthor, QuestResponseCreateArguments arguments)
    {
        if (AuthorId == responseAuthor.Id)
            throw InnerLogicException.QuestExceptions.AuthorCanRespondToQuest(Id, responseAuthor.Id);

        if (State != QuestState.Active || IsOutdated)
            throw InnerLogicException.QuestExceptions.IsNotActive();

        return QuestResponse.New(Id, responseAuthor, arguments);
    }

    public void MakeCompleted(IwentysUser author, IwentysUser executor, QuestCompleteArguments arguments)
    {
        if (AuthorId != author.Id)
            throw InnerLogicException.NotEnoughPermissionFor(author.Id);

        if (State != QuestState.Active || IsOutdated)
            throw InnerLogicException.QuestExceptions.IsNotActive();

        State = QuestState.Completed;
        ExecutorId = executor.Id;
        ExecutorMark = arguments.UserId;
    }

    public static Expression<Func<Quest, bool>> IsCompletedBy(IwentysUser user)
    {
        return q => q.State == QuestState.Completed
                    && q.Responses.Any(r => r.StudentId == user.Id);
    }
}