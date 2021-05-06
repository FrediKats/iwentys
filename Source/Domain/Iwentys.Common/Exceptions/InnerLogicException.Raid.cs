namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class RaidExceptions
        {
            public static InnerLogicException RequestIsNotActual(int raidId, int studentId)
            {
                return new InnerLogicException($"Cannot accept raid request. Invite is not active. Raid: {raidId}, Student: {studentId}");
            }
        }
    }
}
