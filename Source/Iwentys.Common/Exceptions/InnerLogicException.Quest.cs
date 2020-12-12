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
        }
    }
}
