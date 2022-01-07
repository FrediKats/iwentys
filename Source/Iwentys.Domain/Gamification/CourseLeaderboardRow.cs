namespace Iwentys.Domain.Gamification
{
    public class CourseLeaderboardRow
    {
        public int Position { get; set; }
        public int? OldPosition { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
    }
}