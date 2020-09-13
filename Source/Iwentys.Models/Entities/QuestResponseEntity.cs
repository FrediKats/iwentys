using System;

namespace Iwentys.Models.Entities
{
    public class QuestResponseEntity
    {
        public Quest Quest { get; set; }
        public int QuestId { get; set; }

        public StudentEntity Student { get; set; }
        public int StudentId { get; set; }

        public DateTime ResponseTime { get; set; }

        public static QuestResponseEntity New(int questId, int studentId)
        {
            return new QuestResponseEntity
            {
                QuestId = questId,
                StudentId = studentId,
                ResponseTime = DateTime.UtcNow
            };
        }
    }
}