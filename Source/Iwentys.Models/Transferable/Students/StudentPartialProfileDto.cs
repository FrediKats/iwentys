﻿using System;
using Iwentys.Models.Entities;
using Iwentys.Models.Types;

namespace Iwentys.Models.Transferable.Students
{
    public class StudentPartialProfileDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SecondName { get; set; }
        public UserType Role { get; set; }
        public string GithubUsername { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastOnlineTime { get; set; }
        public int BarsPoints { get; set; }

        public StudentPartialProfileDto()
        {
        }

        public StudentPartialProfileDto(Student student)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            MiddleName = student.MiddleName;
            SecondName = student.SecondName;
            Role = student.Role;
            GithubUsername = student.GithubUsername;
            CreationTime = student.CreationTime;
            LastOnlineTime = student.LastOnlineTime;
            BarsPoints = student.BarsPoints;
        }
    }
}