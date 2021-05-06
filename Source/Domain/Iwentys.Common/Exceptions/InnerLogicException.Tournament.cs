namespace Iwentys.Common.Exceptions
{
    public partial class InnerLogicException
    {
        public static class TournamentException
        {
            public static InnerLogicException NoTeamRegistered(int tournamentId)
            {
                return new InnerLogicException($"No team in tournament. Tournament: {tournamentId}");
            }

            public static InnerLogicException IsNotFinished(int tournamentId)
            {
                return new InnerLogicException($"Tournament is not finished. Tournament: {tournamentId}");
            }

            public static InnerLogicException AlreadyFinished(int tournamentId)
            {
                return new InnerLogicException($"Tournament already finished. Tournament: {tournamentId}");
            }
        }
    }
}
