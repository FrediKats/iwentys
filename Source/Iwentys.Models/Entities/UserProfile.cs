using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Iwentys.Models.Types;

namespace Iwentys.Models.Entities
{
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public UserGraduationYear UserGraduationYear { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }
    }
}