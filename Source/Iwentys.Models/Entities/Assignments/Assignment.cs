using System;
using System.ComponentModel.DataAnnotations;

namespace Iwentys.Models.Entities.Assignments
{
    public class Assignment
    {
        [Key]
        public Int32 Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}