namespace Iwentys.Common;

public partial class InnerLogicException
{
    public static class QuestExceptions
    {
        public static InnerLogicException IsNotActive()
        {
            throw new InnerLogicException("Quest is not active");
        }

        public static InnerLogicException AuthorCanRespondToQuest(int questId, int authorId)
        {
            throw new InnerLogicException($"Quest author cannot respond to quest. QuestId: {questId}, AuthorId: {authorId}");
        }
    }
}