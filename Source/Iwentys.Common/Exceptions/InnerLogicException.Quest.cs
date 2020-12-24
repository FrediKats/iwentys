namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class Quest
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
}
